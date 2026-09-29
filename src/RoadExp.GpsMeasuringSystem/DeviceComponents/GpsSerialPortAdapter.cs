using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.HardwareConnections;
using RoadExp.Measurements.Core;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Channels;

namespace RoadExp.GpsMeasuringSystem
{    
    public sealed class GpsSerialPortAdapter : IDeviceAdapter, IAsyncDisposable, IDisposable
    {
        #region const
        private const int _channelCapacity = 1024;
       
        private const byte _lineFeed = (byte)'\n';
        #endregion

        #region private fields       
        private readonly IIndexProvider _indexProvider;
        private readonly ILogger<GpsSerialPortAdapter> _logger;
        private readonly Channel<MeasurementBatch> _channel;

        private readonly IHardwareConnector _serialPort;
        private Pipe? _pipe;
        private CancellationTokenSource? _cts;
        private Task? _readingTask;
        private Task? _processingTask;
        
        private byte[]? _currentGgaBytes;
        private byte[]? _currentRmcBytes;
        private int _isInitialized;
        #endregion

        #region constructor
        public GpsSerialPortAdapter(
           IHardwareConnector serialPort,
            IIndexProvider indexProvider,           
            ILogger<GpsSerialPortAdapter> logger)
        {
            _serialPort = serialPort ?? throw new ArgumentNullException(nameof(serialPort));
            _indexProvider = indexProvider ?? throw new ArgumentNullException(nameof(indexProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var options = new BoundedChannelOptions(_channelCapacity)
            {
                SingleReader = true,
                SingleWriter = true,
                FullMode = BoundedChannelFullMode.DropOldest
            };
            _channel = Channel.CreateBounded<MeasurementBatch>(options);
        }
        #endregion

        #region properties
        public ChannelReader<MeasurementBatch> DataChannel => _channel.Reader;
        #endregion

        #region events
        public event EventHandler<MeasurementErrorEventArgs>? ErrorOccurred;
        #endregion

        #region public methods
        public async Task InitializeAsync(CancellationToken token)
        {
            if (Interlocked.CompareExchange(ref _isInitialized, 1, 0) != 0) return;
           

            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GpsAdapter: Не удалось открыть COM-порт");
                Interlocked.Exchange(ref _isInitialized, 0);
                ErrorOccurred?.Invoke(this, new MeasurementErrorEventArgs(ex.Message, ex));
                throw;
            }

            _cts = new CancellationTokenSource();
            _pipe = new Pipe(new PipeOptions(useSynchronizationContext: false));

            _readingTask = ReadFromPortAsync(_serialPort.BaseStream, _pipe.Writer, _cts.Token);
            _processingTask = ProcessPipeAsync(_pipe.Reader, _cts.Token);
        }

        public async Task DeinitializeAsync()
        {
            if (Interlocked.CompareExchange(ref _isInitialized, 0, 1) != 1) return;

            _channel.Writer.TryComplete();
            _cts?.Cancel();

            if (_serialPort != null && _serialPort.IsOpen)
            {
                try { _serialPort.Close(); } catch { }
            }

            if (_pipe != null) await _pipe.Writer.CompleteAsync().ConfigureAwait(false);

            try
            {
                if (_readingTask != null && _processingTask != null)
                {
                    await Task.WhenAll(_readingTask, _processingTask).ConfigureAwait(false);
                }
            }
            catch (Exception) { }
            finally
            {
                _cts?.Dispose();
                _cts = null;
                _readingTask = null;
                _processingTask = null;
            }
        }

        public void Deinitialize() => DeinitializeAsync().GetAwaiter().GetResult();
        public void Dispose() 
        { 
            Deinitialize(); 
            _serialPort?.Dispose(); 
        }

        public async ValueTask DisposeAsync() 
        { 
            await DeinitializeAsync().ConfigureAwait(false); 
            _serialPort?.Dispose(); 
        }
        #endregion

        #region private methods
        private async Task ReadFromPortAsync(Stream stream, PipeWriter writer, CancellationToken token)
        {
            const int minimumBufferSize = 1024;
            try
            {
                while (!token.IsCancellationRequested)
                {
                    Memory<byte> memory = writer.GetMemory(minimumBufferSize);
                    int bytesRead = await stream.ReadAsync(memory, token).ConfigureAwait(false);
                    if (bytesRead == 0) break;

                    writer.Advance(bytesRead);
                    FlushResult result = await writer.FlushAsync(token).ConfigureAwait(false);
                    if (result.IsCompleted || result.IsCanceled) break;
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критическая ошибка чтения GPS потока.");
                ErrorOccurred?.Invoke(this, new MeasurementErrorEventArgs("Сбой связи с GPS-приемником.", ex));
            }
            finally
            {
                await writer.CompleteAsync().ConfigureAwait(false);
            }
        }

        private async Task ProcessPipeAsync(PipeReader reader, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    ReadResult result = await reader.ReadAsync(token).ConfigureAwait(false);
                    ReadOnlySequence<byte> buffer = result.Buffer;

                    while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
                    {
                        ProcessNmeaLine(line);
                    }

                    reader.AdvanceTo(buffer.Start, buffer.End);
                    if (result.IsCompleted || result.IsCanceled) break;
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                await reader.CompleteAsync().ConfigureAwait(false);
            }
        }

        private bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            line = default;
            SequencePosition? position = buffer.PositionOf(_lineFeed);
            if (position == null) return false;

            line = buffer.Slice(0, position.Value);
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
            return true;
        }

        private void ProcessNmeaLine(in ReadOnlySequence<byte> line)
        {
            if (line.Length < 6) return;

            // Вытягиваем преамбулу на стек без аллокаций
            Span<byte> header = stackalloc byte[6];
            line.Slice(0, 6).CopyTo(header);

            // Проверка сигнатур ASCII: $xxGGA и $xxRMC
            if (header[0] == '$' && header[3] == 'G' && header[4] == 'G' && header[5] == 'A')
            {
                _currentGgaBytes = line.ToArray();
            }
            else if (header[0] == '$' && header[3] == 'R' && header[4] == 'M' && header[5] == 'C')
            {
                _currentRmcBytes = line.ToArray();
            }

            if (_currentGgaBytes != null && _currentRmcBytes != null)
            {
                BuildAndDispatchBatch();
            }
        }

        private void BuildAndDispatchBatch()
        {
            long index = _indexProvider.GetIndex();

            // Создаем бинарный пакет по стандарту:
            // [0] = длина GGA, [1] = длина RMC, [2...] = данные
            byte[] result = new byte[_currentGgaBytes!.Length + _currentRmcBytes!.Length + 2];
            result[0] = (byte)_currentGgaBytes.Length;
            result[1] = (byte)_currentRmcBytes.Length;

            Buffer.BlockCopy(_currentGgaBytes, 0, result, 2, _currentGgaBytes.Length);
            Buffer.BlockCopy(_currentRmcBytes, 0, result, _currentGgaBytes.Length + 2, _currentRmcBytes.Length);

            _channel.Writer.TryWrite(new MeasurementBatch(index, result));

            _currentGgaBytes = null;
            _currentRmcBytes = null;
        }
        #endregion
    }

}
