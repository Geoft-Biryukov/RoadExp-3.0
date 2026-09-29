using RoadExp.Measurements.Core;

namespace RoadExp.GeometryMeasuringSystem
{
    /// <summary>
    /// Параметры настройки 
    /// измерительной системы "Геометрия"
    /// </summary>
    public class GeometrySystemSettings : MeasuringSystemSettings
    {
        private const string _defaultFileName = "geometry.dat";

        public GeometrySystemSettings()
        {
            FileName = _defaultFileName;            
        }

        /// <summary>
        /// Имя COM-порта
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// Скорость передачи данных 
        /// по COM-порту
        /// </summary>
        public int BaudRate { get; set; }
        
    }
}
