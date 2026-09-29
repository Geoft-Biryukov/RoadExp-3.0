using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.TestMeasuringSystem;
using TestMeasuringSystem;

namespace RoadExp.TestMeasuringSystem
{
    public class TestMeasuringSystemManager : IMeasuringSystemManager
    {
        private const string _settingsFileName = "test-system.settings";
        private readonly TestMeasuringSystemSettings _settings;
        private readonly IMeasuringSystem<TestDataItem> _system;
        private readonly IMeasuringSystemSettingsManager<TestMeasuringSystemSettings> _settingsManager;
        private readonly ILogger<TestMeasuringSystemManager> _logger;

        public TestMeasuringSystemManager(
            [FromKeyedServices("TestSystem")]
            IMeasuringSystemSettingsManager<TestMeasuringSystemSettings> settingsManager,
            [FromKeyedServices("TestSystem")] IMeasuringSystemsFactory<TestMeasuringSystemSettings, TestDataItem> systemFactory,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TestMeasuringSystemManager>();

            _settings = settingsManager.LoadOrDefault(_settingsFileName);

            _logger.LogDebug("Настройки загружены");
            _logger.LogDebug($"{nameof(_settings.IsEmulationMode)} = {_settings.IsEmulationMode}");

            _system = systemFactory.CreateMeasuringSystem(_settings);

            _system.DataReceived += SystemDataReceived;
            _settingsManager = settingsManager;
        }
       
        public IMeasuringSystem MeasuringSystem => _system;

        public MeasuringSystemSettings Settings => _settings;

        public bool IsEmulationMode 
        { 
            get => _settings.IsEmulationMode;
            set => _settings.IsEmulationMode = value;
        }

        public string SystemId => throw new NotImplementedException();

        public void Deinitialize()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _system.DataReceived -= SystemDataReceived;
            _system.Dispose();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public void StartAcquisition(string filePath)
        {
            throw new NotImplementedException();
        }

        public void StopAcquisition()
        {
            throw new NotImplementedException();
        }

        public void UpdateParameters()
        {
            throw new NotImplementedException();
        }

        private void SystemDataReceived(object? sender, DataEventArgs<TestDataItem> e)
        {
            Console.WriteLine($"Тестовые данные: {e.Data.Value}");
        }

    }
}
