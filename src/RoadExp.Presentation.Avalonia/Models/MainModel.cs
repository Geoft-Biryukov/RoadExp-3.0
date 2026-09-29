using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Measurements.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.Models
{
    public class MainModel : IDisposable
    {
        #region private fields
        private readonly Dictionary<string, IMeasuringSystemModel> _systemModels = new Dictionary<string, IMeasuringSystemModel>();
        private readonly ILogger _logger;
        private readonly Dictionary<string, IMeasuringSystemManager> _systemManagers = new Dictionary<string, IMeasuringSystemManager>();
        #endregion

        #region constructor
        public MainModel(
            ILogger<MainModel> logger, 
            IEnumerable<IMeasuringSystemModel> systemModels,
            IEnumerable<IMeasuringSystemManager> systemManagers
            )
        {
            Thread.CurrentThread.Name = "GUI Thread";

            SettingsModel = new SettingsModel();           
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            foreach (var systemModel in systemModels)
            {
                RegisterModel(systemModel.SystemId, systemModel);                
            }

            foreach (var manager in systemManagers)
            {
                RegisterSystemManager(manager.SystemId, manager);
            }

            ConfigureErrorHandlers(systemManagers);
        }

       
        #endregion

        #region properties
        public SettingsModel SettingsModel { get; }
        #endregion

        #region public methods
        public void RegisterModel(string systemId,  IMeasuringSystemModel model)
        {
            if (_systemModels.ContainsKey(systemId))
                throw new ArgumentException($"Модель с ключом {systemId} уже зарегистрирована");

            var message = $"Регистрируется {systemId}";
            _logger.LogInformation(message);

            _systemModels.Add(systemId, model);
        }

        public IMeasuringSystemModel GetModel(string systemId)
        {
            if (!_systemModels.TryGetValue(systemId, out IMeasuringSystemModel? value))
                throw new ArgumentException($"Модель с ключом {systemId} отсутствует");

            return value;
        }

        private void RegisterSystemManager(string systemId, IMeasuringSystemManager manager)
        {
            if (_systemManagers.ContainsKey(systemId))                
                throw new ArgumentException($"Менеджер с ключом {systemId} уже зарегистрирована");

            var message = $"Регистрируется {systemId}";
            _logger.LogInformation(message);

            _systemManagers.Add(systemId, manager);
        }

        public IEnumerable<Task> InitializeAll()
        {
            var result = new List<Task>();

            foreach (var manager in _systemManagers.Values)
            {
                result.Add(manager.InitializeAsync());
            }

            return result;
        }

        public void DeinitializeAll()
        {
            foreach (var manager in _systemManagers.Values)
            {
                manager.Deinitialize();
            }
        }
        
        public void StartAcquisition()
        {
            foreach(var manager in _systemManagers.Values)
            {
                manager.StartAcquisition(SettingsModel.Settings.FolderToSaveData);
            }
        }
        
        public void StopAcquisition()
        {
            foreach (var manager in _systemManagers.Values)
            {
                manager.StopAcquisition();
            }
        }

        public void Dispose()
        {
            foreach(var manager in _systemManagers.Values)
            {
                if (manager is IMeasurementErrorProvider errorProvider)
                {
                    errorProvider.ErrorOccurred -= ErrorProviderErrorOccurred;
                }

                manager.Dispose();
            }
        }
        #endregion

        #region private methods
        private void ConfigureErrorHandlers(IEnumerable<IMeasuringSystemManager> systemManagers)
        {
            foreach (var manager in systemManagers)
            {
                if(manager is IMeasurementErrorProvider errorProvider)
                {
                    errorProvider.ErrorOccurred += ErrorProviderErrorOccurred;
                }
            }
        }

        private void ErrorProviderErrorOccurred(object? sender, MeasurementErrorEventArgs e)
        {
            _logger.UserError(e.Message);
        }
        #endregion
    }
}
