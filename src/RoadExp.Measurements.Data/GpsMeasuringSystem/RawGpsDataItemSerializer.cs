using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;

namespace RoadExp.Measurements.Data.GpsMeasuringSystem
{
    public class RawGpsDataItemSerializer : IDataItemSerializer<ByteArrayDataItem>
    {       
        public byte[] Serialize(ByteArrayDataItem item)
        {
            return item.Value;

            //int dataSize = item.Value.Length;
            //int count = sizeof(long) + sizeof(int) + dataSize; // size of index + size of length + length of data

            //var result = new byte[count];

            //byte[] indexBytes = BitConverter.GetBytes(item.Index);
            //byte[] lengthBytes = BitConverter.GetBytes(dataSize);
            //byte[] data = item.Value;
            
            //Buffer.BlockCopy(indexBytes,0, result, 0, indexBytes.Length);
            //Buffer.BlockCopy(lengthBytes, 0, result, indexBytes.Length, lengthBytes.Length);
            //Buffer.BlockCopy(data, 0, result, indexBytes.Length + lengthBytes.Length, data.Length);

            //return result;
        }
        
        public ByteArrayDataItem Deserialize(byte[] source)
        {
            throw new NotImplementedException();
        }
    }
}
