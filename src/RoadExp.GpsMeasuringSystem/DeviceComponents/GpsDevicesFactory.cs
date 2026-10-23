using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;

namespace RoadExp.GpsMeasuringSystem.DeviceComponents
{
    public class GpsDevicesFactory : IDevicesFactory<GpsSystemSettings>
    {
        #region private fields
        private readonly IDataStorage _storage;
        private readonly IDeviceAdapterFactory<GpsSystemSettings> _adapterFactory;
        private readonly ILoggerFactory _loggerFactory;
        #endregion

        #region constructor
        public GpsDevicesFactory(IDataStorage storage,
            IDeviceAdapterFactory<GpsSystemSettings> adapterFactory,
            ILoggerFactory loggerFactory)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        #endregion

        #region events
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region public methods
        /// <summary>
        /// Создаёт список из одного GpsDevice, сконфигурированного
        /// в соответствии с переданными настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>Список из одного GpsDevice</returns>
        public IReadOnlyList<IDevice> CreateDevices(GpsSystemSettings settings)
        {
            var adapter = _adapterFactory.Create(settings);
            var logger = _loggerFactory.CreateLogger<GpsDevice>();

            return new List<IDevice>
            {
                new GpsDevice(adapter, _storage, logger)
            };
        }
        #endregion
    }
}

