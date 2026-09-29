using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.HardwareConnections;
using RoadExp.Measurements.Core;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Channels;

namespace RoadExp.GeometryMeasuringSystem
{    
    public sealed class GeometrySerialPortAdapter : IDeviceAdapter, IAsyncDisposable, IDisposable
    {
        #region const
        public const int _gkvPacketLength = 60;
        private const int _channelCapacity = 1024;
        #endregion

        #region private fields
        // Преамбула пакета GKV
        private static readonly byte[] _preamble = { 0xFF, 0x01, 0x13 };
        

        private readonly IIndexProvider _indexProvider;
        private readonly ILogger<GeometrySerialPortAdapter> _logger;
        private readonly Channel<MeasurementBatch> _channel;

        private IHardwareConnector? _serialPort;
        private Pipe? _pipe;

        private CancellationTokenSource? _cts;
        private Task? _readingTask;
        private Task? _processingTask;
       
        private int _isInitialized; // 0 = false, 1 = true
        #endregion

        #region constructor
        public GeometrySerialPortAdapter(
            IHardwareConnector serialPort,
            IIndexProvider indexProvider,
            ILogger<GeometrySerialPortAdapter> logger)
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
            if (Interlocked.CompareExchange(ref _isInitialized, 1, 0) != 0)
            {
                throw new InvalidOperationException("Адаптер уже инициализирован.");
            }

            _logger.LogInformation("Инициализация адаптера МИНС");
          

            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось открыть COM-порт");
                Interlocked.Exchange(ref _isInitialized, 0);
                OnErrorOccurred(new MeasurementErrorEventArgs(ex.Message));
                throw;
            }

            _cts = new CancellationTokenSource();

            PipeOptions _pipeOptions = new(pauseWriterThreshold: 65536, resumeWriterThreshold: 32768, useSynchronizationContext: false);
            _pipe = new Pipe(_pipeOptions);

            _readingTask = ReadFromPortAsync(_serialPort.BaseStream, _pipe.Writer, _cts.Token);
            _processingTask = ProcessPipeAsync(_pipe.Reader, _cts.Token);            
        }

        public async Task DeinitializeAsync()
        {
            if (Interlocked.CompareExchange(ref _isInitialized, 0, 1) != 1)
                return;

            _logger.LogInformation("Деинициализация адаптера...");
            
            if (_cts != null)
            {
                _cts.Cancel();
            }

            if (_serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Ошибка при закрытии COM-порта.");
                }
            }

            if (_pipe != null)
            {
                await _pipe.Writer.CompleteAsync().ConfigureAwait(false);
            }

            try
            {
                if (_readingTask != null && _processingTask != null)
                {
                    // Используем Task.WhenAny с таймаутом для абсолютной надежности
                    var timeoutTask = Task.Delay(2000);
                    var completedTask = await Task.WhenAny(Task.WhenAll(_readingTask, _processingTask), timeoutTask).ConfigureAwait(false);

                    if (completedTask == timeoutTask)
                    {
                        _logger.LogWarning("Фоновые задачи не завершились за 2 секунды. Принудительное завершение.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Исключение при завершении фоновых задач (нормально при закрытии порта).");
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
                _readingTask = null;
                _processingTask = null;
            }
        }

        public void Deinitialize()
        {
            DeinitializeAsync().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _channel.Writer.TryComplete();

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
        private async Task ReadFromPortAsync(Stream serialStream, PipeWriter writer, CancellationToken token)
        {
            const int minimumBufferSize = 512;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    Memory<byte> memory = writer.GetMemory(minimumBufferSize);

                    int bytesRead = await serialStream.ReadAsync(memory, token).ConfigureAwait(false);
                    if (bytesRead == 0) break;

                    writer.Advance(bytesRead);

                    FlushResult result = await writer.FlushAsync(token).ConfigureAwait(false);
                    if (result.IsCompleted || result.IsCanceled) break;
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критическая ошибка при чтении из COM-порта.");
                OnErrorOccurred(new MeasurementErrorEventArgs(ex.Message));
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

                    while (TrySequenceParse(ref buffer, out ReadOnlySequence<byte> packet))
                    {
                        ProcessPacket(packet);
                    }

                    reader.AdvanceTo(buffer.Start, buffer.End);

                    if (result.IsCompleted || result.IsCanceled) break;
                }
            }
            catch (OperationCanceledException) 
            { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критическая ошибка при обработке данных буфера.");
            }
            finally
            {
                await reader.CompleteAsync().ConfigureAwait(false);
            }
        }

        private bool TrySequenceParse(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> packet)
        {
            packet = default;

            while (buffer.Length >= _gkvPacketLength)
            {                
                SequencePosition? matchPos = buffer.PositionOf(_preamble[0]);
                if (matchPos == null)
                {
                    buffer = buffer.Slice(buffer.End);
                    return false;
                }

                buffer = buffer.Slice(matchPos.Value);

                if (buffer.Length < _gkvPacketLength) return false;

                // Проверяем всю преамбулу (3 байта) целиком в Span без аллокаций
                Span<byte> localPreamble = stackalloc byte[_preamble.Length];
                buffer.Slice(0, _preamble.Length).CopyTo(localPreamble);

                if (localPreamble.SequenceEqual(_preamble))
                {
                    packet = buffer.Slice(0, _gkvPacketLength);
                    buffer = buffer.Slice(_gkvPacketLength);
                    return true;
                }

                // Если 0xFF был ложным сигналом, сдвигаемся на 1 байт вперед
                buffer = buffer.Slice(1);
            }

            return false;
        }

        private void ProcessPacket(in ReadOnlySequence<byte> packet)
        {
            long timeIndex = _indexProvider.GetIndex();
            byte[] rawBytes = packet.ToArray();           

            var dataItem = new MeasurementBatch(timeIndex, rawBytes);
            _channel.Writer.TryWrite(dataItem);
        }
       
        private void OnErrorOccurred(MeasurementErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        #endregion
    }
}
