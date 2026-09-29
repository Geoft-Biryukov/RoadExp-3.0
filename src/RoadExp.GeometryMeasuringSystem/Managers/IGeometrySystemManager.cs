using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;

namespace RoadExp.GeometryMeasuringSystem
{
    public interface IGeometrySystemManager : IMeasuringSystemManager
    {
        string PortName { get; set; }

        int BaudRate { get; set; }

        event EventHandler<DataEventArgs<GeometryDataItem>> DataReceived;
    }
}
