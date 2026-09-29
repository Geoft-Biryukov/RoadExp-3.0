using RoadExp.Measurements.Core;
using RoadExp.Utils;

namespace RoadExp.DistanceMeasuringSystem.DataConverters
{
    /// <summary>
    /// Преобразование DistanceRawData -> byte[] и
    /// byte[] -> DistanceRawData
    /// </summary>
    public class DistanceDataConverter : IByteArrayConverter<DistanceRawData>
    {
        public DistanceRawData FromByteArray(byte[] source)
        {
            return StructConverter.ByteArrayToStruct<DistanceRawData>(source);
        }

        public byte[] ToByteArray(DistanceRawData rawData)
        {
            return StructConverter.StructToByteArray(rawData);
        }
    }
}
