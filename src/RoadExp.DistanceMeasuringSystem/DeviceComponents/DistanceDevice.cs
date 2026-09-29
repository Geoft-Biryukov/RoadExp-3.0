using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;

namespace RoadExp.DistanceMeasuringSystem.DeviceComponents
{
    public class DistanceDevice : Device
    {
        public DistanceDevice(
            IDeviceAdapter adapter,
            IDataStorage storage,
            ILogger<DistanceDevice> logger) : base(adapter, storage, logger)
        { }
    }
}
