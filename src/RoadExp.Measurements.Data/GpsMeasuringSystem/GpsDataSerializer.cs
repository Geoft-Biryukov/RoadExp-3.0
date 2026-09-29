using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Serializable;
using RoadExp.Utils;

namespace RoadExp.Measurements.Data.GpsMeasuringSystem
{
    public class GpsDataSerializer : IDataItemSerializer<GpsDataItem>
    {
        public byte[] Serialize(GpsDataItem data)
        {
            var gpsData = GpsDataConverters.Convert(data);

            return StructConverter.StructToByteArray(gpsData);
        }

        public GpsDataItem Deserialize(byte[] source)
        {
            var data = StructConverter.ByteArrayToStruct<TGPSData>(source);

            GpsData gpsData = GpsDataConverters.Convert(data);

            return new GpsDataItem(data.Index, gpsData);
        }
    }
}
