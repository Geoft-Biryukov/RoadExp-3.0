using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;

namespace RoadExp.DistanceMeasuringSystem.MeasuringSystem
{
    public class DistanceSystemFactory : IMeasuringSystemsFactory<DistanceSystemSettings, DistanceDataItem>
    {
        #region private fields
        private readonly IDevicesFactory<DistanceSystemSettings> _devicesFactory;        
        private readonly IByteArrayParser<DistanceDataItem> _parser;
        private readonly IFileHeaderFactory _headerFactory;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        #endregion

        #region constructor
        public DistanceSystemFactory(
            IDevicesFactory<DistanceSystemSettings> devicesFactory,           
            IByteArrayParser<DistanceDataItem> parser,
            IFileHeaderFactory headerFactory,
            ILoggerFactory loggerFactory)
        {
            _devicesFactory = devicesFactory;           
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _headerFactory = headerFactory ?? throw new ArgumentNullException(nameof(headerFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = _loggerFactory.CreateLogger<DistanceSystemFactory>();
        }
        #endregion

        #region public methods
        /// <summary>
        /// Создает измерительную систему ДПП
        /// и настраивает её в соответствии с переданными
        /// настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>Настроенная измерительная система ДПП</returns>
        public IMeasuringSystem<DistanceDataItem> CreateMeasuringSystem(DistanceSystemSettings settings)
        {
            _logger.LogInformation("Создается и настраивается система ДПП");            
            
            var logger = _loggerFactory.CreateLogger<DistanceSystem>();

            return new DistanceSystem(_devicesFactory, settings, _parser, _headerFactory, logger);
        }
        #endregion
    }
}
