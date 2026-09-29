using System.Globalization;

namespace RoadExp.GpsMeasuringSystem.DataParsers
{
    public static class NmeaHelpers
    {
        // Проверка контрольной суммы NMEA
        public static bool ValidateChecksum(string sentence)
        {
            if (string.IsNullOrEmpty(sentence) || !sentence.StartsWith("$"))
                return false;

            int starPos = sentence.IndexOf('*');
            if (starPos == -1 || starPos + 2 >= sentence.Length)
                return false;

            byte checksum = 0;
            for (int i = 1; i < starPos; i++)
                checksum ^= (byte)sentence[i];

            string provided = sentence.Substring(starPos + 1, 2);
            byte providedChecksum = byte.Parse(provided, NumberStyles.HexNumber);
            return checksum == providedChecksum;
        }

        // Конвертация формата ddmm.mmmm в десятичные градусы
        public static double ConvertNmeaToDecimal(string nmeaCoord, string direction)
        {
            if (string.IsNullOrEmpty(nmeaCoord)) return double.NaN;

            double value = double.Parse(nmeaCoord, CultureInfo.InvariantCulture);
            int degrees = (int)(value / 100);
            double minutes = value - degrees * 100;
            double decimalDeg = degrees + minutes / 60.0;

            if (direction == "S" || direction == "W")
                decimalDeg = -decimalDeg;

            return decimalDeg;
        }

        // Парсинг времени из hhmmss.ss
        public static TimeSpan ParseTime(string timeStr)
        {
            if (string.IsNullOrEmpty(timeStr))
                return TimeSpan.Zero;

            double seconds = double.Parse(timeStr, CultureInfo.InvariantCulture);
            int hours = (int)(seconds / 10000);
            int minutes = (int)((seconds - hours * 10000) / 100);
            int secs = (int)(seconds - hours * 10000 - minutes * 100);
            int millis = (int)((seconds - Math.Floor(seconds)) * 1000);

            return new TimeSpan(0, hours, minutes, secs, millis);
        }

        // Парсинг даты из ddmmyy
        public static DateTime ParseDate(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr) || dateStr.Length != 6)
                return DateTime.MinValue;

            int day = int.Parse(dateStr.Substring(0, 2));
            int month = int.Parse(dateStr.Substring(2, 2));
            int year = 2000 + int.Parse(dateStr.Substring(4, 2));
            return new DateTime(year, month, day);
        }
    }
}
