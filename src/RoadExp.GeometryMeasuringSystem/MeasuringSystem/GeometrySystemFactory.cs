using Microsoft.Extensions.Logging;
using RoadExp.GeometryMeasuringSystem.DeviceComponents;
using RoadExp.GeometryMeasuringSystem.MeasuringSystem;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;

namespace RoadExp.GeometryMeasuringSystem
{
    public class GeometrySystemFactory : IMeasuringSystemsFactory<GeometrySystemSettings, GeometryDataItem>
    {
        private readonly IDevicesFactory<GeometrySystemSettings> _devicesFactory;
        private readonly IByteArrayParser<GeometryDataItem> _parser;
        private readonly IFileHeaderFactory _headerFactory;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public GeometrySystemFactory(
            IDevicesFactory<GeometrySystemSettings> devicesFactory,
            IByteArrayParser<GeometryDataItem> parser,
            IFileHeaderFactory headerFactory,
            ILoggerFactory loggerFactory)
        {
            _devicesFactory = devicesFactory ?? throw new ArgumentNullException(nameof(devicesFactory));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _headerFactory = headerFactory ?? throw new ArgumentNullException(nameof(headerFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            _logger = _loggerFactory.CreateLogger<GeometrySystemFactory>();
        }

        /// <summary>
        /// Создает измерительную систему геометрия
        /// и настраивает её в соответствии с переданными
        /// настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>Настроенная измерительная система геометрия</returns>
        public IMeasuringSystem<GeometryDataItem> CreateMeasuringSystem(GeometrySystemSettings settings)
        {
            _logger.LogInformation("Создается и настраивается система Геометрия");                                    

            var logger = _loggerFactory.CreateLogger<GeometrySystem>();

            return new GeometrySystem(_devicesFactory, settings, _parser, _headerFactory, logger);
        }

    }

}
