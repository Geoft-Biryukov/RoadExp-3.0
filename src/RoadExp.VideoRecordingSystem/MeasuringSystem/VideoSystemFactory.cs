using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.VideoRecordingSystem;

namespace RoadExp.VideoRecordingSystem.MeasuringSystem
{
    public class VideoSystemFactory : IMeasuringSystemsFactory<VideoSystemSettings, VideoDataItem>
    {
        #region private fields
        private readonly IDevicesFactory<VideoSystemSettings> _devicesFactory;
        private readonly IByteArrayParser<VideoDataItem> _parser;
        private readonly IFileHeaderFactory _headerFactory;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        #endregion

        #region constructor
        public VideoSystemFactory(
            IDevicesFactory<VideoSystemSettings> devicesFactory,
            IByteArrayParser<VideoDataItem> parser,
            IFileHeaderFactory headerFactory,
            ILoggerFactory loggerFactory)
        {
            _devicesFactory = devicesFactory;
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _headerFactory = headerFactory ?? throw new ArgumentNullException(nameof(headerFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = _loggerFactory.CreateLogger<VideoSystemFactory>();
        }
        #endregion

        #region public methods
        /// <summary>
        /// Создает измерительную систему Видеосъемка
        /// и настраивает её в соответствии с переданными
        /// настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>Настроенная измерительная система Видеосъемка</returns>
        public IMeasuringSystem<VideoDataItem> CreateMeasuringSystem(VideoSystemSettings settings)
        {
            _logger.LogInformation("Создается и настраивается система Видеосъемка");

            var logger = _loggerFactory.CreateLogger<VideoSystem>();

            return new VideoSystem(_devicesFactory, settings, _parser, _headerFactory, logger);
        }
        #endregion
    }
}
