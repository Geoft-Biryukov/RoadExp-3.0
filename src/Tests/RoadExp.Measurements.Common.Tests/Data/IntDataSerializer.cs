using RoadExp.Measurements.Core.Serialization;

namespace RoadExp.Measurements.Common.Tests.Data
{
    internal class IntDataSerializer : IDataItemSerializer<IntData>
    {
        public byte[] Serialize(IntData item)
        {
            return BitConverter.GetBytes(item.Value);
            
        }
        public IntData Deserialize(byte[] source)
        {
            throw new NotImplementedException();
        }

    }
}
