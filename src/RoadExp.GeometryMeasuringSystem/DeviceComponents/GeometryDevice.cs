using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;

namespace RoadExp.GeometryMeasuringSystem
{
    public class GeometryDevice : Device
    {
        public GeometryDevice(
            IDeviceAdapter adapter,
            IDataStorage storage,
            ILogger<GeometryDevice> logger)
            : base(adapter, storage, logger)
        {
        }
    }

}
