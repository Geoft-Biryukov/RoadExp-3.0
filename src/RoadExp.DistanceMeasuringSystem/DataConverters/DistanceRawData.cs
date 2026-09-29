using System.Runtime.InteropServices;

namespace RoadExp.DistanceMeasuringSystem.DataConverters
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DistanceRawData
    {
        public byte FakeByte;

        /// <summary>
        /// счетчик основного датчика пути
        /// </summary>
        public UInt16 MainWayCounter;

        /// <summary>
        /// счетчик дополнительного датчика пути
        /// </summary>
        public UInt16 OptionalWayCounter;

        /// <summary>
        /// счетчик для толчкомера
        /// </summary>
        public UInt16 RovnCounter;

        /// <summary>
        /// напряжение питания аккумуляторной батареи в десятых долях вольта
        /// </summary>
        public UInt16 PowerBatteryValue;

        /// <summary>
        /// состояние питания всех систем побитово
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] SystemPowerState;

        /// <summary>
        /// счетчик импульсов основного датчика пути между сигналами Z(полный оборот энкодера)
        /// </summary>
        public UInt16 MainWayCheckCounter;

        /// <summary>
        /// версия прошивки контроллера
        /// </summary>
        public byte Version;

        /// <summary>
        /// зарезервировано на будущее
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Reserved;
    }
}
