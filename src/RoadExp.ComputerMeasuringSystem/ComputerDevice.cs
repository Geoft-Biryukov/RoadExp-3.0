using RoadExp.ComputerMeasuringSystem.Data;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;

namespace RoadExp.ComputerMeasuringSystem
{
    public class ComputerDevice : Device<ComputerDataItem>
    {
        public ComputerDevice(IDeviceAdapter<ComputerDataItem> adapter, IDataStorage<ComputerDataItem> storage)
            : base(adapter, storage)
        { }
    }
}
