using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.ComputerMeasuringSystem.Data;
using RoadExp.Measurements.Core;

namespace RoadExp.ComputerMeasuringSystem
{
    public class ComputerSystem : IMeasuringSystem<ComputerDataItem>
    {
        private readonly IDevice<ComputerDataItem> _device;
        private readonly ILogger _logger;


        public ComputerSystem(IDevice<ComputerDataItem> device, MeasuringSystemSettings settings, ILogger logger)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ComputerDataItem HotValue => _device.HotValue;

        public bool IsActive { get; private set; }

        public MeasuringSystemSettings Settings { get; }

        public event EventHandler<DataEventArgs<ComputerDataItem>> DataReceived
        {
            add { _device.DataReceived += value; }
            remove { _device.DataReceived -= value; }
        }

        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        
        public async Task InitializeAsync()
        {
            _logger.UserInformation($"Выполняется инициализация {nameof(ComputerSystem)}");

            await _device.InitializeAsync();

            _logger.UserInformation($"Инициализация {nameof(ComputerSystem)} успешно завершена");
        }

        public void Deinitialize()
        {
            _device.Deinitialize();
        }

        public void Dispose()
        {
            Deinitialize();
        }

        public void OpenFile()
        {
            //_device.OpenFile(Settings.FileName);
        }

        public void StartAcquisition()
        {
            _device.StartAcquisition();            
        }

        public void StopAcquisition()
        {
            _device.StopAcquisition();            
        }

        public void UpdateAdapter()
        {
            
        }
    }
}
