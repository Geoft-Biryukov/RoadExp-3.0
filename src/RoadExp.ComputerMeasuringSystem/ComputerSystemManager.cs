using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoadExp.ComputerMeasuringSystem.Data;
using RoadExp.Measurements.Core;

namespace RoadExp.ComputerMeasuringSystem
{
    public class ComputerSystemManager : IMeasuringSystemManager
    {
        private const string _settingsFileName = "computer.settings";
        private readonly ComputerSystemSettings _settings;
        private readonly IMeasuringSystem<ComputerDataItem> _system;
        private readonly IMeasuringSystemSettingsManager<ComputerSystemSettings> _settingsManager;
        private readonly ILogger<ComputerSystemManager> _logger;

        public ComputerSystemManager(
            [FromKeyedServices("Computer")] 
            IMeasuringSystemSettingsManager<ComputerSystemSettings> settingsManager,
            [FromKeyedServices("Computer")] IMeasuringSystemsFactory<ComputerSystemSettings, ComputerDataItem> systemFactory,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ComputerSystemManager>();

            _settings = settingsManager.LoadOrDefault(_settingsFileName);
            
            _logger.LogDebug("Настройки загружены");
            _logger.LogDebug($"{nameof(_settings.IsEmulationMode)} = {_settings.IsEmulationMode}");

            _system = systemFactory.CreateMeasuringSystem(_settings);

            _system.DataReceived += ComputerDataReceived;
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
            _settingsManager.Save(_settings, "computer.settings");
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
            
        }

        private void ComputerDataReceived(object? sender, DataEventArgs<ComputerDataItem> e)
        {
            var performance = e.Data.Value.Performance;
            Console.WriteLine($"Time: {DateTime.Now:HH:mm:ss} | Processor: {performance.CpuUsage,3} | Memory: {performance.AvailableRam}");
        }

    }
}
