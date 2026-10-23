using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Measurements.Core;
using System.Threading.Channels;

namespace RoadExp.GeometryMeasuringSystem
{
    /// <summary>
    /// Адаптер эмулирующий работу навигационной системы
    /// </summary>
    public class GeometryEmulationAdapter : IDeviceAdapter
    {
        private readonly IIndexProvider _indexProvider;
        private readonly ILogger<GeometryEmulationAdapter> _logger;
        private readonly NavigationSimulator _simulator;

        private const int _channelCapacity = 1024;
        private readonly Channel<MeasurementBatch> _channel;


        public GeometryEmulationAdapter(IIndexProvider indexProvider, ILogger<GeometryEmulationAdapter> logger)
        {
            _indexProvider = indexProvider;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _simulator = new NavigationSimulator(speedKmH: 55);

            var options = new BoundedChannelOptions(_channelCapacity)
            {
                SingleReader = true,
                SingleWriter = true,
                FullMode = BoundedChannelFullMode.DropOldest
            };

            _channel = Channel.CreateBounded<MeasurementBatch>(options);
        }
        public ChannelReader<MeasurementBatch> DataChannel => _channel.Reader;

        #region events      
        public event EventHandler<Measurements.Core.MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        public async Task InitializeAsync(CancellationToken token)
        {
            _logger.GeometryInformation("Выполняется инициализация эмуляции");
            await Task.Run(() => Initialize(), token);
        }

        public void Deinitialize()
        {
            _logger.GeometryInformation("Выполняется деинициализация эмуляции");
            _simulator.StopNavigation();
            _simulator.RawDataGenerated -= SimulatorRawDataGenerated;
        }

        public void Dispose()
        {
            Deinitialize();

            _channel.Writer.TryComplete();
        }

        private void Initialize()
        {
            _simulator.RawDataGenerated += SimulatorRawDataGenerated;
            var route = CreateRoute();
            _simulator.StartNavigation(route);
        }

        private List<GeoPoint> CreateRoute()
        {
            var route = new List<GeoPoint>();

            route.Add(new GeoPoint(51.590132, 45.934918));
            route.Add(new GeoPoint(51.593338, 45.918433));
            route.Add(new GeoPoint(51.604726, 45.876791));

            return route;
        }

        private void SimulatorRawDataGenerated(byte[] data)
        {
            long index = _indexProvider.GetIndex();

            var item = new MeasurementBatch(index, data);

            _channel.Writer.TryWrite(item);
        }
    }

}
