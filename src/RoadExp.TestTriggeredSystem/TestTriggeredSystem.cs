using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.SystemsConnection;
using RoadExp.Measurements.Data.TestTriggeredSystem;

namespace RoadExp.TestTriggeredSystem
{
    public class TestTriggeredSystem : IMeasuringSystem<TestTriggeredDataItem>, ITriggered
    {
        private readonly IDevice<TestTriggeredDataItem> _device;
        private readonly ITriggered _triggered;
        private readonly TestTriggeredSettings _settings;


        public TestTriggeredSystem(IDevice<TestTriggeredDataItem> device, TestTriggeredSettings settings)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            _triggered = device as ITriggered;

            if (_triggered == null)
            {
                throw new ArgumentException(nameof(device));
            }
        }

        public event EventHandler<DataEventArgs<TestTriggeredDataItem>> DataReceived
        {
            add { _device.DataReceived += value; }
            remove { _device.DataReceived -= value; }
        }
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
       
        public async Task InitializeAsync()
        {
            await _device.InitializeAsync();
            IsActive = true;
        }

        public void Deinitialize()
        {
            _device.Deinitialize();
            IsActive = false;
        }

        public bool IsActive { get; private set; } = false;

        public MeasuringSystemSettings Settings => _settings;

        public TestTriggeredDataItem HotValue => _device.HotValue;

        public void Dispose()
        {
            //_device.Dispose();
        }

        public void StartAcquisition()
        {
            _device.StartAcquisition();
        }

        public void StopAcquisition()
        {
            _device.StopAcquisition();
        }

        public void Trigger()
        {
            _triggered.Trigger();
        }

        public void OpenFile()
        {
            //_device.OpenFile(Settings.FileName);
        }

        public void UpdateAdapter()
        {
            
        }
    }
}
