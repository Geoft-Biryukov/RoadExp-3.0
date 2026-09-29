using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.SystemsConnection;
using RoadExp.Measurements.Data.TestTriggeredSystem;

namespace RoadExp.TestTriggeredSystem
{
    public class TestTriggeredDevice : Device<TestTriggeredDataItem>, ITriggered
    {
        private ITriggered _triggeredAdapter;

        public TestTriggeredDevice(IDeviceAdapter<TestTriggeredDataItem> adapter, IDataStorage<TestTriggeredDataItem> storage)
            : base(adapter, storage)
        {
            _triggeredAdapter = adapter as ITriggered;

            if (_triggeredAdapter == null)
                throw new ArgumentException("Неправильный тип адаптера");
        }
        public void Trigger()
        {
            _triggeredAdapter.Trigger();
        }
    }
}
