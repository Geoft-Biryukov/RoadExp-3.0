using RoadExp.Measurements.Core.Serialization;
using System.Text;

namespace RoadExp.Measurements.Common
{
    public class FileHeaderFactory : IFileHeaderFactory
    {
        private const ushort _version = 1;
        private readonly byte[] _uniqueCode = Encoding.ASCII.GetBytes("ROAD");

        public CommonFileHeader CreateFileHeader(ushort systemId, ushort metadataSize)
        {
            return new CommonFileHeader
            {
                UniqueCode = _uniqueCode,
                Version = _version,
                TimeUtc = DateTime.UtcNow.Ticks,
                SystemId = systemId,
                MetadataSize = metadataSize,
            };
        }
    }
}
