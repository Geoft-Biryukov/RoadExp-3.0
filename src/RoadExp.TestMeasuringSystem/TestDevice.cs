using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.TestMeasuringSystem;

namespace TestMeasuringSystem
{
    public class TestDevice : Device<TestDataItem>
    {
        public TestDevice(IDeviceAdapter<TestDataItem> adapter, IDataStorage<TestDataItem> dataStorage)
            : base(adapter, dataStorage) 
        {
           
        }      
    }
}
