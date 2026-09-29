using System.IO.Ports;

namespace RoadExp.Utils
{
    /// <summary>
    /// Вспомогательные методы для работы с COM-портом
    /// </summary>
    public static class SerialPortHelpers
    {
        /// <summary>
        /// Возвращает массив имен доступных
        /// портов на данном компьютере
        /// </summary>
        /// <returns>Массив имён доступных COM-портов</returns>
        public static string[] GetNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Возвращает массив доступных скоростей
        /// передачи данных по COM-порту
        /// </summary>
        public static int[] BaudRates
            => new int[] { 4800, 9600, 19200, 57600, 115200, 230400, 460800, 921600 };
    }
}
