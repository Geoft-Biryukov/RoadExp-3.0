using System.Runtime.InteropServices;

namespace RoadExp.Measurements.Data.GpsMeasuringSystem.Serializable
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TGPSTime
    {
        /// <summary>
        /// Час
        /// </summary>
        public byte hours;
        /// <summary>
        /// Минута
        /// </summary>
        public byte minutes;
        /// <summary>
        /// Секунда
        /// </summary>
        public byte seconds;
        /// <summary>
        /// Сотая секунды
        /// </summary>
        public byte ssec;
    }
}
