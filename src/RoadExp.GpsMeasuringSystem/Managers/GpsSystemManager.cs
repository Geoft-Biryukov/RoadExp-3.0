using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;

namespace RoadExp.GpsMeasuringSystem
{
    public class GpsSystemManager : IGpsSystemManager, IMeasurementErrorProvider
    {
        #region private fields
        private const string _settingsFileName = "gps-system.settings";
        private readonly IMeasuringSystemSettingsManager<GpsSystemSettings> _settingsManager;
        private readonly ILogger _logger;        
        private readonly GpsSystemSettings _settings;
        private readonly IMeasuringSystem<GpsDataItem> _system;    
        private bool _isInitializing = false;
        #endregion

        #region constructor
        public GpsSystemManager(            
            IMeasuringSystemSettingsManager<GpsSystemSettings> settingsManager,           
            IMeasuringSystemsFactory<GpsSystemSettings, GpsDataItem> systemFactory,
            ILogger<GpsSystemManager> logger)
        {
            _settingsManager = settingsManager;
            _logger = logger;

            _settings = settingsManager.LoadOrDefault(_settingsFileName);
            _system = systemFactory.CreateMeasuringSystem(_settings);

            _system.ErrorOccurred += SystemErrorOccurred;
        }

        
        #endregion

        #region properties
        public string SystemId => "Gps";
        
        public IMeasuringSystem MeasuringSystem => _system;

        public MeasuringSystemSettings Settings => _settings;

        public string PortName
        {
            get => _settings.PortName; 
            set => _settings.PortName = value;
        }

        public int BaudRate
        { 
            get => _settings.BaudRate; 
            set => _settings.BaudRate = value; 
        }
        
        public bool IsEmulationMode 
        { 
            get => _settings.IsEmulationMode;
            set => _settings.IsEmulationMode = value; 
        }        
        #endregion

        #region events
        public event EventHandler<DataEventArgs<GpsDataItem>> DataReceived;
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region public methods
        public void UpdateParameters()
        {
            if(_system.IsActive)
            {
                _system.Deinitialize();
            }            
        }

        public async Task InitializeAsync()
        {
            if(_system.IsActive || _isInitializing)
                return;

            _isInitializing = true;

            _logger.GpsInformation("Запущена инициализация системы GPS");
            _system.DataReceived += GpsDataReceived;
            await _system.InitializeAsync();
            _logger.GpsInformation("Инициализация системы GPS завершена");

            _isInitializing = false;
        }

       
        public void Deinitialize()
        {
            _logger.GpsInformation("Выполняется деинициализация системы GPS");
            _system.DataReceived -= GpsDataReceived;
            _system.Deinitialize();
        }

        public void StartAcquisition(string filePath)
        {            
            _settings.Path = filePath;

            _logger.UserInformation("Старт записи в файл gps");
            
            _system.StartAcquisition();
        }

        public void StopAcquisition()
        {
            _logger.UserInformation("Стоп записи в файл gps");

            _system.StopAcquisition();                       
        }

        public void Dispose()
        {
            _settingsManager.Save(_settings, _settingsFileName);
            _system.ErrorOccurred -= SystemErrorOccurred;
            _system.Dispose();            
        }
        #endregion     

        #region evend handlers
        private void GpsDataReceived(object? sender, DataEventArgs<GpsDataItem> e)
        {
            DataReceived?.Invoke(this, e);
        }

        private void SystemErrorOccurred(object? sender, MeasurementErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(sender, e);
        }
        #endregion
    }
}
