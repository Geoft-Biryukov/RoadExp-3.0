using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Presentation.Avalonia.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        #region private fields
        private readonly ILogger _logger;
        #endregion

        #region constructor
        public HomePageViewModel(MainModel model, ILogger<HomePageViewModel> logger) : base(model)
        {
            _logger = logger;

            CanInitialize = true;
            CanDeinitialize = false;
            CanStart = false;
            CanStart = false;
        }
        #endregion

        #region properties
        [ObservableProperty]
        private bool _isInitializing;

        [ObservableProperty]
        private bool _canInitialize;

        [ObservableProperty]
        private bool _canDeinitialize;

        [ObservableProperty]
        private bool _canStart;

        [ObservableProperty]
        private bool _canStop;
        #endregion

        #region binding methods
        public async Task Initialize()
        {
            IsInitializing = true;
            _logger.UserInformation("Выполняется инициализация всех систем");

            await Task.Run(() =>
            {

                var tasks = Model.InitializeAll();

                Task.WaitAll(tasks.ToArray());
            });

            IsInitializing = false;
            _logger.UserInformation("Инициализация всех систем успешно выполнена");

            UpdateButtonsState(nameof(Initialize));
        }

        public void Deinitialize()
        {
            Model.DeinitializeAll();
            _logger.UserInformation("Деинициализация всех систем успешно выполнена");

            UpdateButtonsState(nameof(Deinitialize));
        }

        public void StartAcquisition()
        {
            _logger.UserInformation("Начата регистрация данных всех измерительных устройств");

            Model.StartAcquisition();

            UpdateButtonsState(nameof(StartAcquisition));
        }

        public void StopAcquisition()
        {
            _logger.UserInformation("Завершена регистрация данных всех измерительных устройств");

            UpdateButtonsState(nameof(StopAcquisition));

            Model.StopAcquisition();
        }
        #endregion

        #region private methods

        private void UpdateButtonsState(string methodName)
        {
            switch (methodName)
            {
                case nameof(Initialize):
                    CanInitialize = false;
                    CanDeinitialize = true;
                    CanStart = true;
                    CanStop = false;
                    break;

                case nameof(Deinitialize):
                    CanDeinitialize = false;
                    CanInitialize = true;
                    CanStart = false;
                    CanStop = false;
                    break;

                case nameof(StartAcquisition):
                    CanInitialize = false;
                    CanDeinitialize = false;
                    CanStart = false;
                    CanStop = true;
                    break;

                case nameof(StopAcquisition):
                    CanInitialize = false;
                    CanDeinitialize = true;
                    CanStart = true;
                    CanStop = false;
                    break;

                default:
                    throw new NotSupportedException();
            }

        }

        #endregion
    }
}
