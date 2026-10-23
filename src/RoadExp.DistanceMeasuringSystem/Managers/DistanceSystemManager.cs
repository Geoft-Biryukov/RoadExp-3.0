using Microsoft.Extensions.Logging;
using RoadExp.DistanceMeasuringSystem.MeasuringSystem;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;

namespace RoadExp.DistanceMeasuringSystem.Managers
{
    public class DistanceSystemManager : IDistanceSystemManager, IMeasurementErrorProvider
    {
        #region private fields
        private const string _settingsFileName = "distance-system.settings";
        private readonly IMeasuringSystemSettingsManager<DistanceSystemSettings> _settingsManager;
        private readonly ILogger<DistanceSystemManager> _logger;
        private readonly DistanceSystemSettings _settings;
        private readonly IMeasuringSystem<DistanceDataItem> _system;
        #endregion

        #region constructor
        public DistanceSystemManager(
            IMeasuringSystemSettingsManager<DistanceSystemSettings> settingsManager,
            IMeasuringSystemsFactory<DistanceSystemSettings, DistanceDataItem> systemFactory,
            ILogger<DistanceSystemManager> logger)
        {
            _settingsManager = settingsManager;

            _settings = settingsManager.LoadOrDefault(_settingsFileName);
            _system = systemFactory.CreateMeasuringSystem(_settings);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _system.ErrorOccurred += SystemErrorOccurred;
        }
        #endregion

        #region events
        public event EventHandler<DataEventArgs<DistanceDataItem>> DataReceived;
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region properties
        public IMeasuringSystem MeasuringSystem => _system;

        public MeasuringSystemSettings Settings => _settings;

        public bool IsEmulationMode
        {
            get => _settings.IsEmulationMode;
            set => _settings.IsEmulationMode = value;
        }       

        public string SystemId => "Distance";
        #endregion

        #region public methods
        public async Task InitializeAsync()
        {
            if (_system.IsActive)
                return;
            
            _system.DataReceived += DistanceDataReceived;
            await _system.InitializeAsync();            
        }

        public void Deinitialize()
        {
            _system.DataReceived -= DistanceDataReceived;
            _system.Deinitialize();
        }

        public void StartAcquisition(string filePath)
        {
            _settings.Path = filePath;            

            _logger.LogInformation("Старт записи в файл distance");

            _system.StartAcquisition();
        }

        public void StopAcquisition()
        {
            _logger.LogInformation("Стоп записи в файл distance");

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

        #region event handlers
        private void DistanceDataReceived(object? sender, DataEventArgs<DistanceDataItem> e)
        {
            DataReceived?.Invoke(sender, e);
        }

        private void SystemErrorOccurred(object? sender, MeasurementErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(sender, e);
        }
        #endregion
    }
}
