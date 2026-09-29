namespace RoadExp.Measurements.Data.GpsMeasuringSystem.Data
{
    /// <summary>
    /// Данные сообщения GGA (Global Positioning System Fix Data)
    /// </summary>
    public class GgaData
    {
        /// <summary>
        /// Время UTC (hhmmss.ss) — дата не определена
        /// </summary>
        public DateTime UtcTime { get; set; }

        /// <summary>
        /// Широта, градусы
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота, градусы
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Качество: 0-невалидно, 1-GPS, 2-DGPS, 4-RTK и т.д
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// Количество используемых спутников
        /// </summary>
        public int Satellites { get; set; }

        /// <summary>
        /// Horizontal Dilution of Precision
        /// </summary>
        public double HDOP { get; set; }

        /// <summary>
        /// Высота над уровнем моря (метры)
        /// </summary>
        public double Altitude { get; set; }

        /// <summary>
        /// Разделение геоида (метры)
        /// </summary>
        public double GeoidalSeparation { get; set; }

        /// <summary>
        /// Возраст DGPS (секунды)
        /// </summary>
        public double AgeOfDgps { get; set; }

        /// <summary>
        /// ID DGPS-станции
        /// </summary>
        public int DgpsStationId { get; set; }
    }
}
