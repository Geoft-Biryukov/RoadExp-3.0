using System.Runtime.InteropServices;

namespace RoadExp.Measurements.Core.Serialization
{    
    /// <summary>
    /// Описательная часть пакета 
    /// (всегда фиксированного размера - 16 байт)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketHeader
    {
        public ushort SyncWord;       // 0xAABB
        public long Index;            // Int64 Ticks
        public uint Counter;          // Счетчик данных
        public ushort PayloadLength;  // Длина данных
    }
}
