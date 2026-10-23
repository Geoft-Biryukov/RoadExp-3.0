using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.VideoRecordingSystem;
using RoadExp.VideoRecordingSystem.MeasuringSystem;

namespace RoadExp.VideoRecordingSystem.Managers
{
    public class VideoSystemManager : IVideoSystemManager
    {
        #region private fields
        private const string _settingsFileName = "video-system.settings";
        private readonly IMeasuringSystemSettingsManager<VideoSystemSettings> _settingsManager;
        private readonly ILogger<VideoSystemManager> _logger;
        private readonly VideoSystemSettings _settings;
        private readonly IMeasuringSystem<VideoDataItem> _system;
        #endregion

        #region constructor
        public VideoSystemManager(
            IMeasuringSystemSettingsManager<VideoSystemSettings> settingsManager,
            IMeasuringSystemsFactory<VideoSystemSettings, VideoDataItem> systemFactory,
            ILogger<VideoSystemManager> logger)
        {
            _settingsManager = settingsManager;

            _settings = settingsManager.LoadOrDefault(_settingsFileName);
            _settings.CameraCount = 3; // Для отладки
            _settings.IsEmulationMode = true; // Для отладки
            _system = systemFactory.CreateMeasuringSystem(_settings);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _system.ErrorOccurred += SystemErrorOccurred;
        }
        #endregion

        #region properties
        public string SystemId => nameof(VideoSystem);

        public IMeasuringSystem MeasuringSystem => _system;

        public MeasuringSystemSettings Settings => _settings;

        public bool IsEmulationMode
        {
            get => _settings.IsEmulationMode;
            set => _settings.IsEmulationMode = value;
        }
        #endregion

        #region public methods
        public async Task InitializeAsync()
        {
            if (_system.IsActive)
                return;

            _system.DataReceived += VideoDataReceived;
            await _system.InitializeAsync();
        }

        public void Deinitialize()
        {
            _system.DataReceived -= VideoDataReceived;
            _system.Deinitialize();
        }

        public void StartAcquisition(string filePath)
        {
            _settings.Path = filePath;

            _logger.LogInformation("Старт записи в файл video");

            _system.StartAcquisition();
        }

        public void StopAcquisition()
        {
            _logger.LogInformation("Стоп записи в файл video");

            _system.StopAcquisition();
        }

        public void UpdateParameters()
        {
            
        }

        public void Dispose()
        {
            _settingsManager.Save(_settings, _settingsFileName);
            _system.ErrorOccurred -= SystemErrorOccurred;
            _system.Dispose();
        }

        
        #endregion

        #region private methods
        private void SystemErrorOccurred(object? sender, MeasurementErrorEventArgs e)
        {
            
        }

        private void VideoDataReceived(object? sender, DataEventArgs<VideoDataItem> e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
