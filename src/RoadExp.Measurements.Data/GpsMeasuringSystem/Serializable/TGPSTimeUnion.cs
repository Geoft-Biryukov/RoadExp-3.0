using System.Runtime.InteropServices;

namespace RoadExp.Measurements.Data.GpsMeasuringSystem.Serializable
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public class TGPSTimeUnion
    {
        [FieldOffset(0)]
        public int SatelliteTime;

        [FieldOffset(0)]
        public TGPSTime tm;
    }
}
