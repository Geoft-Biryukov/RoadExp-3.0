using Newtonsoft.Json;
using RoadExp.Measurements.Core;

namespace RoadExp.GpsMeasuringSystem
{
    /// <summary>
    /// Настройки измерительной системы Gps
    /// </summary>
    public class GpsSystemSettings : MeasuringSystemSettings
    {
        private const string _defaultFileName = "gps.dat";

        public GpsSystemSettings()
        {
            FileName = _defaultFileName;
        }

        /// <summary>
        /// Имя COM-порта, к которому подключена измерительная система
        /// </summary>
        public string PortName {  get; set; }

        /// <summary>
        /// Скорость передачи данных по COM-порту
        /// </summary>
        public int BaudRate { get; set; }       
        
    }
}
