using RoadExp.Measurements.Data.GeometryMeasuringSystem;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;

namespace RoadExp.GeometryMeasuringSystem
{
    /// <summary>
    /// Предоставляет вспомогательные методы 
    /// для работы с данными геометрии
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Парсинг массива байтов, поступающего от МИНС
        /// в данные геометрии
        /// </summary>
        /// <param name="src">Массив байтов от МИНС</param>
        /// <returns>Данные геометрии</returns>
        public static GeometryData ParseData(byte[] src)
        {
            int srcOffset = 8;

            int dataCount = 12;

            int dataSize = sizeof(float);

            float[] dataArray = new float[dataCount];

            Buffer.BlockCopy(src, srcOffset, dataArray, 0, dataCount * dataSize);

            var ret = new GeometryData
            {
                PacketNum = (ushort)dataArray[0],
                LongHeel = dataArray[1],
                CrossHeel = dataArray[2],
                Course = dataArray[3],
                GPSTime = dataArray[4],
                Latitude = dataArray[5],
                Longitude = dataArray[6],
                HeightGPS = dataArray[7],
                SpeedGPS = dataArray[8],
                SatellitesCount = (int)dataArray[9],
                CourseGPS = dataArray[10],
                VerticalSpeedGPS = dataArray[11]
            };

            return ret;

        }
    }
}
