namespace RoadExp.Measurements.Data.GpsMeasuringSystem.Data
{
    /// <summary>
    /// Расшифрованные данные NMEA-0183    
    /// </summary>
    public class GpsData
    {
        public GpsData(long systemTime, GgaData gga, RmcData rmc)
        {
            SystemTime = systemTime;
            Gga = gga;
            Rmc = rmc;
        }

        /// <summary>
        /// Сообщение NMEA-0183 типа GGA (расшифрованное) 
        /// </summary>
        public GgaData Gga { get; }

        /// <summary>
        /// Сообщение NMEA-0183 типа RMC (расшифрованное) 
        /// </summary>
        public RmcData Rmc { get; }

        /// <summary>
        /// Системное время в момент регистрации
        /// данных gps
        /// </summary>
        public long SystemTime { get; }
    }
}
