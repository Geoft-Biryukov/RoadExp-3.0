using System.Runtime.InteropServices;

namespace RoadExp.Common
{
    /// <summary>
    /// Структура информации о дате
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TDateIzmer
    {
        /// <summary>
        /// день
        /// </summary>
        public byte Day;
        /// <summary>
        /// месяц
        /// </summary>
        public byte Month;
        /// <summary>
        /// год
        /// </summary>
        public ushort Year;
    };
}
