namespace RoadExp.GpsMeasuringSystem
{
    public class GpsRawData
    {
        public GpsRawData(string[] rawNmeaStrings)
        {
            Nmea = rawNmeaStrings;
        }

        public string[] Nmea { get; }
    }
}
