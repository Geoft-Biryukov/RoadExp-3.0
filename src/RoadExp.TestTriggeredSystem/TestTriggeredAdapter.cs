using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.SystemsConnection;
using RoadExp.Measurements.Data.TestTriggeredSystem;

namespace RoadExp.TestTriggeredSystem
{
    public class TestTriggeredAdapter : IDeviceAdapter<TestTriggeredDataItem>, ITriggered
    {
        private int _counter = 0;

        public event EventHandler<DataEventArgs<TestTriggeredDataItem>> DataReceived;
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;

        private void Initialize()
        {
            Thread.Sleep(3000);
        }
        
        public async Task InitializeAsync(CancellationToken token)
        {
            await Task.Run(() => Initialize(), token);
        }

        public void Deinitialize()
        {
            
        }

        public void Trigger()
        {
            long index = DateTime.UtcNow.Ticks;

            var data = new TestTriggeredDataItem(index, _counter--);
            var args = new DataEventArgs<TestTriggeredDataItem>(data);

            DataReceived?.Invoke(this, args);
        }

        public void Dispose()
        {
            
        }
    }
}
