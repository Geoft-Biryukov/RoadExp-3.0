using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using System;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.Models
{
    public class ComputerModel : IMeasuringSystemModel
    {
        //private readonly ComputerSystemSettings _settings = new ComputerSystemSettings
        //{
        //    FileName = "C:/Tmp/computer.txt",
        //    ElementsCount = 1,
        //};
        private readonly ILogger<ComputerModel> _logger;
        private readonly ILoggerFactory _loggerFactory;
      //  private ComputerSystem _system;

        public ComputerModel(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<ComputerModel>();
        }

        public void CreateSystem()
        {
            //var factory = new ComputerSystemFactory(_loggerFactory);

            //_system = factory.CreateMeasuringSystem(_settings) as ComputerSystem;

            //if (_system == null)
            //{
            //    _logger.UserError("Не удалось создать систему");
            //}
            //else
            //{
            //    _logger.UserInformation("Система успешно создана");
            //}
        }

        public event EventHandler DataReady;

        public ComputerData Data { get; private set; }

        public string SystemId => "ComputerSystem";

        public async Task InitializeSystemAsync()
        {
            //if (_system == null)
            //{
            //    _logger.UserError("Система не создана. Нажмите кнопку создать");
            //    return;
            //}

            //await _system.InitializeAsync();

            //_logger.UserInformation("Инициализация системы завершена");

            //_system.DataReceived += SystemDataReceived;
        }

        public void StartAcquisition()
        {
           // _system.StartAcquisition();
            _logger.UserInformation("Начата регистрация");
        }

        public void StopAcquisition()
        {
            //_system.StopAcquisition();

            _logger.UserInformation("Регистрация остановлена");
        }

        //private void SystemDataReceived(object? sender, DataEventArgs<ComputerDataItem> e)
        //{
        //    var performance = e.Data.Value.Performance;
        //    var data = new ComputerData(performance.CpuUsage, performance.AvailableRam);
        //    Data = data;
        //    DataReady?.Invoke(this, EventArgs.Empty);
        //}
    }
}
