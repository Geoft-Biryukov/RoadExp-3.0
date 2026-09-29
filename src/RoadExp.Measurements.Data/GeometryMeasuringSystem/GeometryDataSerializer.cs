using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Serializable;
using RoadExp.Utils;

namespace RoadExp.Measurements.Data.GeometryMeasuringSystem
{
    /// <summary>
    /// Сериализует данные, измеренные навигационной системой
    /// в массив байт
    /// </summary>
    public class GeometryDataSerializer : IDataItemSerializer<GeometryDataItem>
    {
        public byte[] Serialize(GeometryDataItem item)
        {
            var data = GeometryDataConverters.Convert(item);

            return StructConverter.StructToByteArray(data);
        }
        
        public GeometryDataItem Deserialize(byte[] source)
        {
            var data = StructConverter.ByteArrayToStruct<TGeometryData>(source);

            return GeometryDataConverters.Convert(data);
            
        }
    }
}
