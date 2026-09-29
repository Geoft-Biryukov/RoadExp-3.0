using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Serializable;

namespace RoadExp.Measurements.Data.GpsMeasuringSystem
{
    public static class GpsDataConverters
    {
        public static TGPSData Convert(IDataItem<GpsData> data)
        {
            ArgumentNullException.ThrowIfNull(data);

            var result = new TGPSData();

            result.Index = data.Index;

            var gga = data.Value.Gga;
            var rmc = data.Value.Rmc;

            result.smPosition = 0;

            result.Latitude = gga.Latitude;
            result.Longitude = gga.Longitude;
            result.Height = (float)gga.Altitude;

            result.Goodness = (byte)gga.Satellites;
            result.Status = (byte)gga.Quality;
            result.DiffAge = gga.AgeOfDgps;

            result.Course = rmc.Course;
            result.Speed = (float)rmc.SpeedKnots;
            result.SatelliteTime = ConvertTime(rmc.UtcTime);
            result.SystemTime = (ulong)data.Value.SystemTime;

            return result;
        }

        public static GpsData Convert(TGPSData data)
        {
            var gga = new GgaData
            {
                Latitude = data.Latitude,
                Longitude = data.Longitude,
                Altitude = data.Height, 
                Satellites = data.Goodness,
                Quality = data.Status,
                AgeOfDgps = data.DiffAge,
            };

            var rmc = new RmcData
            {
                Course = data.Course,
                SpeedKnots = data.Speed,
                UtcTime = ConvertTime(data.SatelliteTime),
            };

            return new GpsData((long)data.SystemTime, gga, rmc);
        }

        private static TGPSTimeUnion ConvertTime(DateTime time)
        {
            TGPSTimeUnion result = new TGPSTimeUnion();

            result.tm.hours = (byte)time.Hour;
            result.tm.minutes = (byte)time.Minute;
            result.tm.seconds = (byte)time.Second;
            result.tm.ssec = (byte)time.Millisecond;

            return result;
        }

        private static DateTime ConvertTime(TGPSTimeUnion time)
        {
            DateOnly dateOnly = new DateOnly();
            TimeOnly timeOnly = new TimeOnly(time.tm.hours, time.tm.minutes, time.tm.seconds, time.tm.ssec);
            DateTime result = new DateTime(dateOnly, timeOnly);

            return result;
        }
    }
}
