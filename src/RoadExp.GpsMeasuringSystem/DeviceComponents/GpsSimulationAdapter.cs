using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Measurements.Core;
using System.Threading.Channels;


namespace RoadExp.GpsMeasuringSystem
{
    // <summary>
    /// Адаптер, эмулирующий работу gps
    /// </summary>
    public class GpsSimulationAdapter : IDeviceAdapter
    {
        #region private fields
        private const int _channelCapacity = 1024;
        private const double _msToS = 1e-3;
        private readonly IIndexProvider _indexProvider;
        private readonly IByteArrayConverter<GpsRawData> _converter;
        private readonly ILogger<GpsSimulationAdapter> _logger;
        private int _period = 100;
        private Timer _timer;
        private double _time;
        private GpsSimulator _simulator;

        private readonly Channel<MeasurementBatch> _channel;

        #endregion

        #region constructor
        public GpsSimulationAdapter(IIndexProvider indexProvider, IByteArrayConverter<GpsRawData> converter, ILogger<GpsSimulationAdapter> logger)
        {
            _indexProvider = indexProvider ?? throw new ArgumentNullException(nameof(indexProvider));
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _time = _period * _msToS;

            var options = new BoundedChannelOptions(_channelCapacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = true,
            };

            _channel = Channel.CreateBounded<MeasurementBatch>(options);
        }
        #endregion

        public ChannelReader<MeasurementBatch> DataChannel => _channel.Reader;

        #region events        
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region public methods
        public Task InitializeAsync(CancellationToken token)
        {
            _logger.GpsInformation("Выполняется инициализация GpsSimulationAdapter");
            return Task.Run(() => Initialize());
        }

        public void Deinitialize()
        {
            _logger.GpsInformation("Выполняется деинициализация GpsSimulationAdapter");
            _timer?.Dispose();
        }


        public void Dispose()
        {
            Deinitialize();
        }
        #endregion

        #region private methods
        private void Initialize()
        {
            _timer = new Timer(TickHandler, null, 0, _period);
            _simulator = new GpsSimulator(51.1, 46.0, 20, 52);
            Thread.Sleep(5000);
        }

        private void TickHandler(object state)
        {
            _simulator.UpdatePosition(_time);

            var dataArray = new string[2];

            dataArray[0] = _simulator.GenerateGga();
            dataArray[1] = _simulator.GenerateRmc();

            var index = _indexProvider.GetIndex();

            var gpsByteArrayData = _converter.ToByteArray(new GpsRawData(dataArray));

            var dataItem = new MeasurementBatch(index, gpsByteArrayData);

            _channel.Writer.TryWrite(dataItem);
        }
        #endregion
    }

}
