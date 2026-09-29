using System.Text;

namespace RoadExp.Measurements.Data
{
    public static class FileHeaderFactory
    {
        public static FileHeader Create(SystemIds id)
        {
            return new FileHeader
            {
                UniqueCode = Encoding.ASCII.GetBytes("ROAD"),
                SystemId = (ushort)id,
                TimeUtc = DateTime.UtcNow.Ticks,
                Version = 0xAA,
            };
        }
    }
}
