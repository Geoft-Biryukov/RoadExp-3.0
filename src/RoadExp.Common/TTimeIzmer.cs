using System.Runtime.InteropServices;

namespace RoadExp.Common
{
    /// <summary>
    /// Структура информации о времени
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TTimeIzmer
    {
        /// <summary>
        /// часы
        /// </summary>
        public byte Hours;
        /// <summary>
        /// минуты
        /// </summary>
        public byte Minutes;
    };
}
