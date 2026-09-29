using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;

namespace RoadExp.DistanceMeasuringSystem.Managers
{
    public interface IDistanceSystemManager : IMeasuringSystemManager
    {
        event EventHandler<DataEventArgs<DistanceDataItem>> DataReceived;
    }
}