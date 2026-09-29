using System.Runtime.InteropServices;

namespace RoadExp.Measurements.Core.Serialization
{
    /// <summary>
    /// Общий заголовок для файлов всех систем
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CommonFileHeader
    {
        /// <summary>
        /// Уникальный код
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] UniqueCode; 

        /// <summary>
        /// Версия формата данных
        /// </summary>
        public ushort Version;

        /// <summary>
        /// ID измерительной системы
        /// </summary>
        public ushort SystemId;

        /// <summary>
        /// Время UTC начала регистрации 
        /// </summary>
        public long TimeUtc;

        /// <summary>
        /// Размер метаданных измерительной системы
        /// </summary>
        public ushort MetadataSize;
    }
}
