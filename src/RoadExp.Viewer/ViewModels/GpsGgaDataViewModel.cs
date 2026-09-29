using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;

namespace RoadExp.Viewer.ViewModels
{
    public class GpsGgaDataViewModel
    {        
        public GpsGgaDataViewModel(long index, GgaData dataItem)
        {            
            Index = index.ToString();
            UtcTime = dataItem.UtcTime.ToString("T");
            Latitude = dataItem.Latitude.ToString("0.00000");
            Longitude = dataItem.Longitude.ToString("0.00000");
            Altitude = dataItem.Altitude.ToString("0.00");
        }

        public string Index { get; }

        public string UtcTime { get; }

        public string Latitude { get; }

        public string Longitude { get; }

        public string Altitude { get; }
    }
}
