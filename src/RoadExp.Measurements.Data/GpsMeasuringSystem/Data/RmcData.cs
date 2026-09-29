namespace RoadExp.Measurements.Data.GpsMeasuringSystem.Data
{
    /// <summary>
    /// Данные сообщения RMC (Recommended Minimum Specific GNSS Data)
    /// </summary>
    public class RmcData
    {
        /// <summary>
        /// Полное UTC время (из времени + даты)
        /// </summary>
        public DateTime UtcTime { get; set; }

        /// <summary>
        /// Статус: true = A (активно), false = V (невалидно)
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Широта, градусы
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота, градусы
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Скорость в узлах
        /// </summary>
        public double SpeedKnots { get; set; }

        /// <summary>
        /// Курс в градусах (истинный)
        /// </summary>
        public double Course { get; set; }

        /// <summary>
        /// Дата (из поля ddmmyy)
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Магнитное склонение (градусы)
        /// </summary>
        public double MagneticVariation { get; set; }

        /// <summary>
        /// Режим (A=автономный, D=дифференциальный, E=оценка, N=невалидно)
        /// </summary>
        public string Mode { get; set; }
    }
}
