using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;

namespace RoadExp.VideoRecordingSystem.DeviceComponents
{
    public class VideoDevice : Device
    {
        public VideoDevice(
            IDeviceAdapter adapter, 
            IDataStorage storage,
            int id,
            ILogger<VideoDevice> logger) 
            : base(adapter, storage, logger)
        {          
            Id = id;
        }

        public int Id { get; }
    }
}
