using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;

namespace RoadExp.GpsMeasuringSystem
{
    public class GpsDevice : Device
    {
        public GpsDevice(
            IDeviceAdapter adapter,
            IDataStorage storage,
            ILogger<GpsDevice> logger)
            : base(adapter, storage, logger)
        { }
    }

}
