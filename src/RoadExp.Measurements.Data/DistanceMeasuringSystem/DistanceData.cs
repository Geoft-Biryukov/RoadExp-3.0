namespace RoadExp.Measurements.Data.DistanceMeasuringSystem
{
    /// <summary>
    /// Обработанные данные ДПП
    /// </summary>
    public class DistanceData
    {
        /// <summary>
        /// Количество импульсов, шт.
        /// </summary>
        public int ImpulseCount { get; set; }

        /// <summary>
        /// Текущее значение пути, см.
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Текущее значение скорости, км/ч
        /// </summary>
        public double Speed { get; set; }

    }
}
