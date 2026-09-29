using RoadExp.Measurements.Data.DistanceMeasuringSystem;

namespace RoadExp.Viewer.ViewModels
{
    public class DistanceDataViewModel
    {
        public DistanceDataViewModel(DistanceDataItem dataItem)
        {            
            Index = dataItem.Index.ToString();

            var data = dataItem.Value;

            ImpulseCount = data.ImpulseCount.ToString();
            Distance = data.Distance.ToString("0.000");
            Speed = data.Speed.ToString("0.00");
        }

        public string Index { get; }

        public string ImpulseCount { get; }

        public string Distance { get; }

        public string Speed { get; }
    }
}
