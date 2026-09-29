using System.Globalization;

namespace RoadExp.GpsMeasuringSystem
{
    /// <summary>
    /// Эмуляция данных GPS
    /// </summary>
    public class GpsSimulator
    {
        #region private fields
        private readonly Random rand = new Random();
        private double _latitude;
        private double _longitude;
        private double _speedKnots;
        private double _courseDegrees;
        #endregion

        #region constructor
        public GpsSimulator(double startLat, double startLon, double speedKnots, double courseDegrees)
        {
            _latitude = startLat;
            _longitude = startLon;
            _speedKnots = speedKnots;
            _courseDegrees = courseDegrees;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Обновление позиции на основе скорости и направления (вызывать по таймеру раз в секунду)
        /// </summary>
        /// <param name="timeStepSeconds">Интервал обновления</param>
        public void UpdatePosition(double timeStepSeconds = 1.0)
        {
            double deltaSpeedKnotes = (rand.NextDouble() - 0.5) * 5;

            _speedKnots += deltaSpeedKnotes;

            _speedKnots = Math.Max(0, _speedKnots);

            // Перевод скорости из узлов в метры в секунду (1 узел = 0.514444 м/с)
            double speedMps = _speedKnots * 0.514444;
            double distanceMoved = speedMps * timeStepSeconds;

            // Радиус Земли в метрах
            const double earthRadius = 6371000.0;

            // Перевод направления в радианы
            double courseRad = _courseDegrees * Math.PI / 180.0;

            // Расчет новых координат (аппроксимация для небольших расстояний)
            double deltaLat = (distanceMoved * Math.Cos(courseRad)) / earthRadius;
            double deltaLon = (distanceMoved * Math.Sin(courseRad)) / (earthRadius * Math.Cos(_latitude * Math.PI / 180.0));

            _latitude += deltaLat * 180.0 / Math.PI;
            _longitude += deltaLon * 180.0 / Math.PI;
        }

        /// <summary>
        /// Генерация строки $GPGGA
        /// </summary>
        /// <returns>Строка формата GGA</returns>
        public string GenerateGga()
        {
            string time = DateTime.UtcNow.ToString("HHmmss.fff", CultureInfo.InvariantCulture);
            string latNmea = ConvertToNmeaFormat(_latitude, true);
            string lonNmea = ConvertToNmeaFormat(_longitude, false);

            // Массив полей GGA
            string nmeaWithoutChecksum = $"GPGGA,{time},{latNmea},{lonNmea},1,08,0.9,54.2,M,0.0,M,,";
            return $"${nmeaWithoutChecksum}*{CalculateChecksum(nmeaWithoutChecksum)}";
        }

        /// <summary>
        /// Генерация строки $GPRMC
        /// </summary>
        /// <returns>Строка формата RMC</returns>
        public string GenerateRmc()
        {
            DateTime utcNow = DateTime.UtcNow;
            string time = utcNow.ToString("HHmmss.fff", CultureInfo.InvariantCulture);
            string date = utcNow.ToString("ddMMyy", CultureInfo.InvariantCulture);
            string latNmea = ConvertToNmeaFormat(_latitude, true);
            string lonNmea = ConvertToNmeaFormat(_longitude, false);
            string speed = _speedKnots.ToString("F2", CultureInfo.InvariantCulture);
            string course = _courseDegrees.ToString("F2", CultureInfo.InvariantCulture);

            // Массив полей RMC
            string nmeaWithoutChecksum = $"GPRMC,{time},A,{latNmea},{lonNmea},{speed},{course},{date},,,A";
            return $"${nmeaWithoutChecksum}*{CalculateChecksum(nmeaWithoutChecksum)}";
        }
        #endregion

        #region private methods
        // Конвертация Decimal Degrees в формат NMEA (DDMM.MMMM)
        private string ConvertToNmeaFormat(double coordinate, bool isLatitude)
        {
            char direction = isLatitude
                ? (coordinate >= 0 ? 'N' : 'S')
                : (coordinate >= 0 ? 'E' : 'W');

            coordinate = Math.Abs(coordinate);
            int degrees = (int)coordinate;
            double totalMinutes = (coordinate - degrees) * 60.0;

            // Выделяем целую и дробную часть минут для контроля ведущих нулей
            int minutesInt = (int)totalMinutes;
            double minutesDec = totalMinutes - minutesInt;

            // Склеиваем минуты: ровно 2 знака для целой части + дробная часть без ведущего нуля
            // "F4" для дробной части берется от sub-минут. substring(2) убирает "0."
            string minutesStr = minutesInt.ToString("D2") + minutesDec.ToString("F4", CultureInfo.InvariantCulture).Substring(1);

            string degreeFormat = isLatitude ? "D2" : "D3";
            string degreesStr = degrees.ToString(degreeFormat, CultureInfo.InvariantCulture);

            return $"{degreesStr}{minutesStr},{direction}";
        }

        // Подсчет контрольной суммы NMEA (XOR всех символов между $ и *)
        private string CalculateChecksum(string sentence)
        {
            int checksum = 0;
            foreach (char c in sentence)
            {
                checksum ^= c;
            }
            return checksum.ToString("X2");
        }
        #endregion
    }
}
