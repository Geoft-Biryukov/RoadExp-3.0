using Microsoft.Extensions.Logging;
using RoadExp.DistanceMeasuringSystem.DataConverters;
using RoadExp.Measurements.Core;
using System.Diagnostics;
using System.Threading.Channels;

namespace RoadExp.DistanceMeasuringSystem.DeviceComponents
{
    public class DistanceEmulationAdapter : IDeviceAdapter
    {
        #region private fields
        private const int _channelCapacity = 1024;
        private const double _impulsePerSecond = 165.0;
        private readonly IIndexProvider _indexProvider;
        private readonly IByteArrayConverter<DistanceRawData> _converter;
        private readonly ILogger<DistanceEmulationAdapter> _logger;        

        private readonly Channel<MeasurementBatch> _channel;

        private long _lastTick;
        private double _accumulator;
        private ushort _distance;

        private Task _worker;
        private CancellationTokenSource _cts;
        #endregion

        public DistanceEmulationAdapter(
            IIndexProvider indexProvider, IByteArrayConverter<DistanceRawData> converter,
            ILogger<DistanceEmulationAdapter> logger)
        {
            _indexProvider = indexProvider;
            _converter = converter;
            _logger = logger;            

            _channel = CreateConfiguredChannel(_channelCapacity);
        }

        #region properties
        public ChannelReader<MeasurementBatch> DataChannel => _channel.Reader;
        #endregion

        #region events
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region public methods
        public Task InitializeAsync(CancellationToken token)
        {
            _logger.LogInformation("Выполняется инициализация эмулятора ДПП");

            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null!;
            }

            _cts = new CancellationTokenSource();

            _worker = Task.Run(() => { Worker(_cts.Token); });
            
            return Task.CompletedTask;
        }
        public void Deinitialize()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null!;
            }

            _worker?.GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            
        }
        #endregion

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

        private void Worker(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_lastTick == 0)
                    _lastTick = Stopwatch.GetTimestamp();

                var ts = Stopwatch.GetTimestamp();
                var dtSec = (ts - _lastTick) / (double)Stopwatch.Frequency;
                _lastTick = ts;

                if (dtSec > 0)
                {
                    _accumulator += _impulsePerSecond * dtSec;
                    int add = (int)_accumulator;
                    if (add > 0)
                    {
                        _accumulator -= add;
                        _distance = (ushort)(_distance + add);
                    }
                }

                long index = _indexProvider.GetIndex();

                var rawData = new DistanceRawData
                {
                    MainWayCounter = _distance
                };
                var array = _converter.ToByteArray(rawData);

                var batch = new MeasurementBatch(index, array);

                _channel.Writer.TryWrite(batch);
                
                Thread.Sleep(5);
            }
        }
        #endregion
    }
}
