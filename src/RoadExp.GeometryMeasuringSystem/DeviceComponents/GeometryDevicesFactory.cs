using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;

namespace RoadExp.GeometryMeasuringSystem.DeviceComponents
{
    public class GeometryDevicesFactory : IDevicesFactory<GeometrySystemSettings>
    {
        #region private fields
        private readonly IDataStorage _storage;
        private readonly IDeviceAdapterFactory<GeometrySystemSettings> _adapterFactory;
        private readonly ILoggerFactory _loggerFactory;
        #endregion

        #region constructor
        public GeometryDevicesFactory(IDataStorage storage,
            IDeviceAdapterFactory<GeometrySystemSettings> adapterFactory,
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
        /// Создаёт список из одного GeometryDevice, сконфигурированного
        /// в соответствии с переданными настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>Список из одного GeometryDevice</returns>
        public IReadOnlyList<IDevice> CreateDevices(GeometrySystemSettings settings)
        {
            var adapter = _adapterFactory.Create(settings);
            var logger = _loggerFactory.CreateLogger<GeometryDevice>();

            return new List<IDevice>
            {
                new GeometryDevice(adapter, _storage, logger)
            };
        }
        #endregion
    }
}
