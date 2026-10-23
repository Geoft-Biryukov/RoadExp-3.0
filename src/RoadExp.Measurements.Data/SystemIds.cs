namespace RoadExp.Measurements.Data
{
    public enum SystemIds : ushort
    {
        Unknown = 0,
        Distance = 1,
        Gps = 2,
        Geometry = 3,
        Iri = 4,
        Video = 5,        
    }

    public static class SystemIdsExtensions
    {
        public static ushort ToUint16(this SystemIds id)
        {
            return (ushort)id;
        }
        
    }
}
