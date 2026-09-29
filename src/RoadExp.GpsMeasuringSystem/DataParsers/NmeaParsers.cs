using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;
using System.Globalization;

namespace RoadExp.GpsMeasuringSystem.DataParsers
{
    public static class NmeaParsers
    {
        /// <summary>
        /// Парсинг сообщения GGA
        /// Пример сообщения: $GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47
        /// </summary>
        public static GgaData ParseGGA(string sentence)
        {
            if (!NmeaHelpers.ValidateChecksum(sentence))
                throw new ArgumentException("Неверная контрольная сумма");

            int starPos = sentence.IndexOf('*');
            string body = sentence.Substring(1, starPos - 1);
            string[] fields = body.Split(',');

            if (fields.Length < 15)
                throw new ArgumentException("Недостаточно полей для GGA");

            var data = new GgaData();

            // Время
            data.UtcTime = DateTime.Today.Add(NmeaHelpers.ParseTime(fields[1]));

            // Координаты
            if (!string.IsNullOrEmpty(fields[2]) && !string.IsNullOrEmpty(fields[3]))
                data.Latitude = NmeaHelpers.ConvertNmeaToDecimal(fields[2], fields[3]);

            if (!string.IsNullOrEmpty(fields[4]) && !string.IsNullOrEmpty(fields[5]))
                data.Longitude = NmeaHelpers.ConvertNmeaToDecimal(fields[4], fields[5]);

            // Качество
            if (int.TryParse(fields[6], out var quality))
            {
                data.Quality = quality;
            }

            // Количество спутников
            if (int.TryParse(fields[7], out var satellites))
            {
                data.Satellites = satellites;
            }

            // HDOP
            if (double.TryParse(fields[8], NumberStyles.Float, CultureInfo.InvariantCulture, out var HDOP))
            {
                data.HDOP = HDOP;
            }

            // Высота
            if (double.TryParse(fields[9], NumberStyles.Float, CultureInfo.InvariantCulture, out var altitude))
            {
                data.Altitude = altitude;
            }

            // Геоид
            if (double.TryParse(fields[11], NumberStyles.Float, CultureInfo.InvariantCulture, out var geoidalSeparation))
            {
                data.GeoidalSeparation = geoidalSeparation;
            }

            // Возраст DGPS
            if (double.TryParse(fields[13], NumberStyles.Float, CultureInfo.InvariantCulture, out var ageOfDgps))
            {
                data.AgeOfDgps = ageOfDgps;
            }

            // ID DGPS
            if (int.TryParse(fields[14], out var dgpsStationId))
            {
                data.DgpsStationId = dgpsStationId;
            }
            return data;
        }


        /// <summary>
        /// Парсинг сообщения RMC
        /// Пример сообщения: $GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A
        /// </summary>
        public static RmcData ParseRMC(string sentence)
        {
            if (!NmeaHelpers.ValidateChecksum(sentence))
                throw new ArgumentException("Неверная контрольная сумма");

            int starPos = sentence.IndexOf('*');
            string body = sentence.Substring(1, starPos - 1);
            string[] fields = body.Split(',');

            if (fields.Length < 12)
                throw new ArgumentException("Недостаточно полей для RMC");

            var data = new RmcData();

            // Время
            TimeSpan time = NmeaHelpers.ParseTime(fields[1]);

            // Статус
            data.IsValid = (fields[2] == "A");

            // Координаты
            if (!string.IsNullOrEmpty(fields[3]) && !string.IsNullOrEmpty(fields[4]))
                data.Latitude = NmeaHelpers.ConvertNmeaToDecimal(fields[3], fields[4]);

            if (!string.IsNullOrEmpty(fields[5]) && !string.IsNullOrEmpty(fields[6]))
                data.Longitude = NmeaHelpers.ConvertNmeaToDecimal(fields[5], fields[6]);

            // Скорость
            if (double.TryParse(fields[7], NumberStyles.Float, CultureInfo.InvariantCulture, out var speedKnots))
            {
                data.SpeedKnots = speedKnots;
            }

            // Курс
            if (double.TryParse(fields[8], NumberStyles.Float, CultureInfo.InvariantCulture, out var course))
            {
                data.Course = course;
            }

            // Дата
            data.Date = NmeaHelpers.ParseDate(fields[9]);

            // Объединяем время и дату
            data.UtcTime = data.Date.Add(time);

            // Магнитное склонение
            if (double.TryParse(fields[10], NumberStyles.Float, CultureInfo.InvariantCulture, out var magneticVariation))
            {
                data.MagneticVariation = magneticVariation;
            }
            // Знак склонения: если поле 11 = "W", то отрицательное (опционально)
            if (fields.Length > 11 && fields[11] == "W")
                data.MagneticVariation = -data.MagneticVariation;

            // Режим (опционально)
            if (fields.Length > 12)
                data.Mode = fields[12];

            return data;
        }


    }
}
