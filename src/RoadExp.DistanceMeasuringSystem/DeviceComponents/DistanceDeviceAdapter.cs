using Microsoft.Extensions.Logging;
using RoadExp.DistanceMeasuringSystem.DataConverters;
using RoadExp.Measurements.Core;
using RoadExp.Utils;
using System.Buffers.Binary;
using System.Threading.Channels;

namespace RoadExp.DistanceMeasuringSystem.DeviceComponents
{
    public class DistanceDeviceAdapter : IDeviceAdapter
    {
        #region private fields
        private const int _channelCapacity = 1024;
        private const byte _powerSensorId = 4;
        private const byte _commandIdPower = 0x01;
        private const int _extendedBufferLength = 17;

        private readonly Stream _stream;
        private readonly IIndexProvider _indexProvider;
        private readonly IByteArrayConverter<DistanceRawData> _byteArrayConverter;
        private readonly ILogger<DistanceDeviceAdapter> _logger;
        private readonly byte[] _inputBuffer;

        private Channel<MeasurementBatch> _channel;
        private CancellationTokenSource _cts;
        private Task _worker;        
        #endregion

        #region constructors
        public DistanceDeviceAdapter(Stream stream, 
                                     int inputBufferLength, 
                                     IIndexProvider indexProvider, 
                                     IByteArrayConverter<DistanceRawData> byteArrayConverter,
                                     ILogger<DistanceDeviceAdapter> logger)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inputBufferLength);

            _inputBuffer = new byte[inputBufferLength];
            _indexProvider = indexProvider ?? throw new ArgumentNullException(nameof(indexProvider));
            _byteArrayConverter = byteArrayConverter ?? throw new ArgumentNullException(nameof(byteArrayConverter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            
        }
        #endregion
       
        #region events
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region properties
        public ChannelReader<MeasurementBatch> DataChannel => _channel.Reader;
        #endregion

        public Task InitializeAsync(CancellationToken token)
        {
            _channel = CreateConfiguredChannel(_channelCapacity);
            SwitchPower(_powerSensorId, true);
            
            _cts = new CancellationTokenSource();            
            _worker = Task.Run(() => ProcessStream(_cts.Token), _cts.Token);

            return Task.CompletedTask;
        }

        public void Deinitialize()
        {
            SwitchPower(_powerSensorId, false);

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            
            _worker?.GetAwaiter().GetResult();
            _worker?.Dispose();
            _worker = null!;

            _channel?.Writer.TryComplete();            
        }

        public void Dispose()
        {
            Deinitialize();            
        }

        #region private methods

        private static Channel<MeasurementBatch> CreateConfiguredChannel(int channelCapacity)
        {
            var options = new BoundedChannelOptions(channelCapacity)
            {
                SingleReader = true,
                SingleWriter = true,
                FullMode = BoundedChannelFullMode.DropOldest,
            };

            return Channel.CreateBounded<MeasurementBatch>(options);
        }

        private async Task ProcessStream(CancellationToken token)
        {
            _logger.LogInformation("Stream processing started.");
            Thread.CurrentThread.Name = "Distance Process Thread";
            try
            {
                while (!token.IsCancellationRequested)
                {                    
                    int byteCount = await _stream.ReadAsync(_inputBuffer.AsMemory(), token).ConfigureAwait(false);

                    if (byteCount == 0)
                    {
                        _logger.LogWarning("Stream reached End-of-Stream (0 bytes read).");
                        break;
                    }

                    ProcessData(_inputBuffer.AsSpan(0, byteCount));
                }
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                _logger.LogInformation("Stream processing canceled normally.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing device stream.");
                ErrorOccurred?.Invoke(this, new MeasurementErrorEventArgs(ex.Message, ex));
            }             
        }

        private void ProcessData(in ReadOnlySpan<byte> data)
        {
            var rawData = new DistanceRawData();

            long index = _indexProvider.GetIndex();

            ushort cntVal = 0;
            ushort cntOptVal = 0;

            cntVal = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(1, 2));

            if (data.Length > 3)
                cntOptVal = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(3, 2));

            rawData.MainWayCounter = cntVal;
            rawData.OptionalWayCounter = cntOptVal;


            if (_inputBuffer.Length == _extendedBufferLength)
            {
                rawData.RovnCounter = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(5, 2));
                rawData.PowerBatteryValue = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(7, 2));
                rawData.SystemPowerState = data.Slice(9, 3).ToArray();
                rawData.MainWayCheckCounter = BitConverter.ToUInt16(data.Slice(12, 2));
                rawData.Version = data[14];
                rawData.Reserved = data.Slice(15, 2).ToArray();
            }

            var array = _byteArrayConverter.ToByteArray(rawData);
            var batch = new MeasurementBatch(index, array);

            _channel.Writer.TryWrite(batch);
        }

        private void SwitchPower(byte sensorId, bool on)
        {
            ushort sensorsState = 0;
            sensorsState = on ? BitHelpers.SetBit(sensorsState, sensorId) : BitHelpers.ClearBit(sensorsState, sensorId);
            SendCommand(_commandIdPower, sensorsState);
        }

        private void SendCommand(int id, uint value)
        {
            if (_stream != null)
            {
                byte[] outReport = new byte[256];

                var data = BitConverter.GetBytes(value);
                outReport[0] = 0x00;
                outReport[1] = (byte)id;
                outReport[2] = data[1];
                outReport[3] = data[0];
                _stream.Write(outReport, 0, 5);
            }
        }       
        #endregion
    }
}
