using System.Runtime.InteropServices;

namespace RoadExp.Measurements.Data.GpsMeasuringSystem.Serializable
{
    /// <summary>
    /// Структура данных файла gps
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TGPSData
    {
        /// <summary>
        /// Индекс 
        /// </summary>
        public long Index;

        /// <summary>
        /// Местоположение, cм
        /// </summary>
        public uint smPosition;

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude;

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude;

        /// <summary>
        /// Высота над уровнем моря, м
        /// </summary>
        public float Height;

        /// <summary>
        /// Курс, град. от северного направления
        /// </summary>
        public double Course;

        /// <summary>
        /// Количество спутников (качество сигналов)
        /// </summary>
        public byte Goodness;

        /// <summary>
        /// Показатель качества навигационных данных:
        ///     0 – навигационные данные не достоверны
        ///     1 – навигационные данные достоверны (автономный режим)
        ///     2 – навигационные данные достоверны (дифференциальный режим)
        ///     4 – навигационные данные достоверны (RTK Fix)
        ///     5 – навигационные данные достоверны (RTK Float)
        ///     6 – навигационные данные получены счислением
        /// </summary>
        public byte Status;

        /// <summary>
        /// Возраст дифференциальных поправок. Временной интервал в секундах между 
        ///     текущим временем и опорным временем последнего принятого RTCM сообщения.
        /// </summary>
        public double DiffAge;

        /// <summary>
        /// Скорость, км/ч
        /// </summary>
        public float Speed;

        /// <summary>
        /// Время спутника
        /// </summary>
        public TGPSTimeUnion SatelliteTime;

        /// <summary>
        /// Количество прошедших миллисекунд начиная с 01.01.1970 (Нужна для синхронизации разных модулей)
        /// </summary>
        public ulong SystemTime;

        /// <summary>
        /// Зарезервированные на будущее
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] Reserved;
    }
}
