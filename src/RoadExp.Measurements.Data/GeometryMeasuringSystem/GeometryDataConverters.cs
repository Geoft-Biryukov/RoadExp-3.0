using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Serializable;

namespace RoadExp.Measurements.Data.GeometryMeasuringSystem
{
    public static class GeometryDataConverters
    {
        public static TGeometryData Convert(IDataItem<GeometryData> data)
        {
            ArgumentNullException.ThrowIfNull(data);

            var result = new TGeometryData();

            var value = data.Value;

            result.Index = data.Index;

            result.smPosition = 0;

            result.grCrossHeel = value.CrossHeel;
            result.grLongHeel = value.LongHeel;

            result.grLongHeelLines = new float[8];

            result.Speed = (ushort)value.SpeedGPS;
            result.grCourse = value.Course;

            result.SystemTime = (ulong)value.GPSTime;

            return result;
        }

        public static GeometryDataItem Convert(TGeometryData data)
        {
            var geometryData = new GeometryData
            {
                CrossHeel = data.grCrossHeel,
                LongHeel = data.grLongHeel,
                Course = data.grCourse,
                SpeedGPS = data.Speed,
                GPSTime = data.SystemTime
            };

            return new GeometryDataItem(data.Index, geometryData);            
        }
    }
}
