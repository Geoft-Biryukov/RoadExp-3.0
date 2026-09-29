using RoadExp.Measurements.Core;

namespace RoadExp.GeometryMeasuringSystem
{
    public class GeometryDataConverter : IByteArrayConverter<GeometryRawData>
    {        
        public byte[] ToByteArray(GeometryRawData rawData)
        {
            return rawData.RawByteArray;
        }

        public GeometryRawData FromByteArray(byte[] source)
        {
            return new GeometryRawData(source);
        }
    }
}
