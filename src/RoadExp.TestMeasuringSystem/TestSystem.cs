using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.TestMeasuringSystem;

namespace TestMeasuringSystem
{
    public class TestSystem : IMeasuringSystem<TestDataItem>
    {
        private readonly IDevice<TestDataItem> _device;
        private readonly TestMeasuringSystemSettings _settings;


        public TestSystem(IDevice<TestDataItem> device, TestMeasuringSystemSettings settings)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));            
        }

        public event EventHandler<DataEventArgs<TestDataItem>> DataReceived
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

        public TestDataItem HotValue => _device.HotValue;
        
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

        public void OpenFile()
        {
            //_device.OpenFile(Settings.FileName);
        }
        

        public void UpdateAdapter()
        {
            throw new NotImplementedException();
        }
    }
}
