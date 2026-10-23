using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;

namespace RoadExp.GeometryMeasuringSystem
{
    /// <summary>
    /// Управляет системой "Геометрия"
    /// </summary>
    public class GeometrySystemManager : IGeometrySystemManager, IMeasurementErrorProvider
    {
        #region private fields
        private const string _settingsFileName = "geometry-system.settings";
        private readonly IMeasuringSystemSettingsManager<GeometrySystemSettings> _settingsManager;
        private readonly ILogger<GeometrySystemManager> _logger;
        private readonly GeometrySystemSettings _settings;
        private readonly IMeasuringSystem<GeometryDataItem> _system;        
        #endregion

        #region constructor
        public GeometrySystemManager(            
            IMeasuringSystemSettingsManager<GeometrySystemSettings> settingsManager,           
            IMeasuringSystemsFactory<GeometrySystemSettings, GeometryDataItem> systemFactory,
            ILogger<GeometrySystemManager> logger)
        {
            _settingsManager = settingsManager;

            _settings = settingsManager.LoadOrDefault(_settingsFileName);
            _system = systemFactory.CreateMeasuringSystem(_settings);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _system.ErrorOccurred += SystemErrorOccurred;
        }
        
        #endregion

        #region events
        public event EventHandler<DataEventArgs<GeometryDataItem>> DataReceived;
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

        public string SystemId => "Geometry";
        #endregion

        #region public methods
        public async Task InitializeAsync()
        {
            if (_system.IsActive)
                return;

            _logger.GeometryInformation("Запущена инициализация системы Геометрия");
            _system.DataReceived += GeometryDataReceived;
            await _system.InitializeAsync();
            _logger.GeometryInformation("Инициализация системы Геометрия завершена");
        }
      
        public void Deinitialize()
        {
            _logger.GeometryInformation("Выполняется деинициализация системы Геометрия");
            _system.DataReceived -= GeometryDataReceived;
            _system.Deinitialize();
        }

        public void StartAcquisition(string filePath)
        {                        
            _settings.Path = filePath;            

            _logger.LogInformation("Старт записи в файл geometry");
                       
            _system.StartAcquisition();
        }

        public void StopAcquisition()
        {
            _logger.LogInformation("Стоп записи в файл geometry");

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
        private void GeometryDataReceived(object? sender, DataEventArgs<GeometryDataItem> e)
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
