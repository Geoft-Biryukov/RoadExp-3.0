using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;

namespace RoadExp.GpsMeasuringSystem
{
    /// <summary>
    /// Фабрика измерительной системы Gps
    /// </summary>
    public class GpsSystemFactory : IMeasuringSystemsFactory<GpsSystemSettings, GpsDataItem>
    {
        private readonly IDevicesFactory<GpsSystemSettings> _devicesFactory;
        private readonly IByteArrayParser<GpsDataItem> _dataParser;
        private readonly IFileHeaderFactory _headerFactory;
        private readonly ILoggerFactory _loggerFactory;

        public GpsSystemFactory(
            IDevicesFactory<GpsSystemSettings> devicesFactory,
            IByteArrayParser<GpsDataItem> dataParser,
            IFileHeaderFactory headerFactory,
            ILoggerFactory loggerFactory)
        {
            _devicesFactory = devicesFactory ?? throw new ArgumentNullException(nameof(devicesFactory));
            _dataParser = dataParser ?? throw new ArgumentNullException(nameof(dataParser));
            _headerFactory = headerFactory ?? throw new ArgumentNullException(nameof(headerFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Создает измерительную систему gps
        /// и настраивает её в соответствии с переданными
        /// настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>Настроенная измерительная система gps</returns>
        public IMeasuringSystem<GpsDataItem> CreateMeasuringSystem(GpsSystemSettings settings)
        {            
            var logger = _loggerFactory.CreateLogger<GpsSystem>();

            return new GpsSystem(_devicesFactory, settings, _dataParser, _headerFactory, logger);
        }       
    }
}
