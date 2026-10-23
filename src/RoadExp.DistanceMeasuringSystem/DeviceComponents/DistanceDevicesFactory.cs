using Microsoft.Extensions.Logging;
using RoadExp.DistanceMeasuringSystem.MeasuringSystem;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;

namespace RoadExp.DistanceMeasuringSystem.DeviceComponents
{
    /// <summary>
    /// Фабрика создания Device'ов системы
    /// Датчик пройденного пути    
    /// </summary>
    public class DistanceDevicesFactory : IDevicesFactory<DistanceSystemSettings>
    {
        #region private fields
        private readonly IDataStorage _storage;
        private readonly IDeviceAdapterFactory<DistanceSystemSettings> _adapterFactory;
        private readonly ILoggerFactory _loggerFactory;
        #endregion

        #region constructor
        public DistanceDevicesFactory(
            IDataStorage storage, 
            IDeviceAdapterFactory<DistanceSystemSettings> adapterFactory,
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
        /// Создаёт список из одного DistanceDevice, сконфигурированного
        /// в соответствии с переданными настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>писок из одного DistanceDevice</returns>
        public IReadOnlyList<IDevice> CreateDevices(DistanceSystemSettings settings)
        {
            var adapter = _adapterFactory.Create(settings);
            var logger = _loggerFactory.CreateLogger<DistanceDevice>();

            return new List<IDevice>
            {
                new DistanceDevice(adapter, _storage, logger)
            };
        }
        #endregion
    }
}
