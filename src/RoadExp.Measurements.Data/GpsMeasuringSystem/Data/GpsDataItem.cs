using RoadExp.Measurements.Core;

namespace RoadExp.Measurements.Data.GpsMeasuringSystem.Data
{
    /// <summary>
    /// Индексированные данные gps-приёмника
    /// </summary>
    public class GpsDataItem : IDataItem<GpsData>
    {
        public GpsDataItem(long index, GpsData value)
        {
            Index = index;
            Value = value;
        }

        /// <summary>
        /// Индекс данных
        /// </summary>
        public long Index { get; }

        /// <summary>
        /// Расшифрованные данные NMEA-0183
        /// </summary>
        public GpsData Value { get; }
    }
}
