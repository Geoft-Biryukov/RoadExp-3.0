using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;

namespace RoadExp.GpsMeasuringSystem
{
    public interface IGpsSystemManager : IMeasuringSystemManager       
    {
        string PortName { get; set; }

        int BaudRate { get; set; }

        event EventHandler<DataEventArgs<GpsDataItem>> DataReceived;
    }
}
