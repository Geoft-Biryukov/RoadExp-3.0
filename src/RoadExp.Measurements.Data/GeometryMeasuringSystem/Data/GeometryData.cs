using System.Globalization;

namespace RoadExp.Measurements.Data.GeometryMeasuringSystem.Data
{
    public class GeometryData
    {
        public ushort PacketNum { get; set; }
        public double LongHeel { get; set; }
        public double CrossHeel { get; set; }
        public double Course { get; set; }
        public double GPSTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double HeightGPS { get; set; }
        public double SpeedGPS { get; set; }
        public int SatellitesCount { get; set; }
        public double CourseGPS { get; set; }
        public double VerticalSpeedGPS { get; set; }

        public GeometryData()
        {

        }

        public GeometryData(string geometryData)
        {
            var paramVals = geometryData.Split(';');

            var culture = CultureInfo.GetCultureInfo("ru-RU");

            if (paramVals.Length != 12) { throw new ArgumentException("Не верная длина строки эмуляции"); }
            try
            {
                PacketNum = ushort.Parse(paramVals[0], culture);
                LongHeel = double.Parse(paramVals[1], culture);
                CrossHeel = double.Parse(paramVals[2], culture);
                Course = double.Parse(paramVals[3], culture);
                GPSTime = double.Parse(paramVals[4], culture);
                Latitude = double.Parse(paramVals[5], culture);
                Longitude = double.Parse(paramVals[6], culture);
                HeightGPS = double.Parse(paramVals[7], culture);
                SpeedGPS = double.Parse(paramVals[8], culture);
                SatellitesCount = int.Parse(paramVals[9], culture);
                CourseGPS = double.Parse(paramVals[10], culture);
                VerticalSpeedGPS = double.Parse(paramVals[11], culture);
            }
            catch
            {
                throw new ArgumentException("Ошибка разбора строки эмуляции");
            }
        }

        public override string ToString() => $"{PacketNum}   {LongHeel}   {CrossHeel}   {Course}   {GPSTime}";
    }
}
