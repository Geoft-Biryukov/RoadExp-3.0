using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;

namespace RoadExp.Viewer.ViewModels
{
    public class GpsRmcDataViewModel
    {
        public GpsRmcDataViewModel(long index, RmcData data)
        {
            Index = index.ToString();
            UtcTime = data.UtcTime.ToString("g");
            Latitude = data.Latitude.ToString("0.00000");
            Longitude = data.Longitude.ToString("0.00000");
            Speed = data.SpeedKnots.ToString("0.00");
            Course = data.Course.ToString("0.000");
        }

        public string Index { get;}

        public string UtcTime { get; }

        public string Latitude { get; }

        public string Longitude { get; }

        public string Speed { get; }

        public string Course { get; }
    }
}
