using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;

namespace RoadExp.Viewer.ViewModels
{
    public class GeometryDataViewModel
    {
        public GeometryDataViewModel(GeometryDataItem dataItem)
        {
            Index = dataItem.Index.ToString();

            var data = dataItem.Value;

            PacketNum = data.PacketNum.ToString();
            LongHeel = data.LongHeel.ToString("0.000");
            CrossHeel = data.CrossHeel.ToString("0.000");
            Course = data.Course.ToString("0.000");
            GPSTime = data.GPSTime.ToString();
            Latitude = data.Latitude.ToString("0.00000");
            Longitude = data.Longitude.ToString("0.00000");
            HeightGPS = data.HeightGPS.ToString("0.00");
            SpeedGPS = data.SpeedGPS.ToString("0.00");
            SatellitesCount = data.SatellitesCount.ToString();
            CourseGPS = data.CourseGPS.ToString("0.00");
            VerticalSpeedGPS = data.VerticalSpeedGPS.ToString("0.00");
        }

        public string Index { get; }
        
        public string PacketNum { get; set; }
        
        public string LongHeel { get; set; }
        
        public string CrossHeel { get; set; }
        
        public string Course { get; set; }
        
        public string GPSTime { get; set; }
        
        public string Latitude { get; set; }
        
        public string Longitude { get; set; }
        
        public string HeightGPS { get; set; }
        
        public string SpeedGPS { get; set; }
        
        public string SatellitesCount { get; set; }
        
        public string CourseGPS { get; set; }
        
        public string VerticalSpeedGPS { get; set; }
    }
}
