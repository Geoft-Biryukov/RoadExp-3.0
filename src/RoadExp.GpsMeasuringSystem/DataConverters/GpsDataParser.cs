using RoadExp.GpsMeasuringSystem.DataParsers;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;
using System.Text;

namespace RoadExp.GpsMeasuringSystem
{
    public class GpsDataParser : IByteArrayParser<GpsDataItem>
    {              
        public GpsDataItem Parse(in MeasurementBatch batch)
        {            
            byte[] data = batch.Data;

            if (data == null || data.Length < 2)
            {             
                throw new InvalidOperationException("Неверный формат бинарного пакета GPS.");
            }

            // Быстро вычитываем длины фреймов из заголовка
            int lengthGga = data[0];
            int lengthRmc = data[1];

            if (data.Length < 2 + lengthGga + lengthRmc)
            {             
                throw new InvalidOperationException("Размер бинарного пакета GPS не совпадает с заголовком.");
            }

            // Создаем строки ОДИН раз непосредственно перед парсингом текста NMEA
            string ggaString = Encoding.ASCII.GetString(data, 2, lengthGga);
            string rmcString = Encoding.ASCII.GetString(data, 2 + lengthGga, lengthRmc);

            var gga = NmeaParsers.ParseGGA(ggaString);
            var rmc = NmeaParsers.ParseRMC(rmcString);

            var gpsData = new GpsData(Environment.TickCount64, gga, rmc);

            return new GpsDataItem(batch.Index, gpsData);
        }
    }
}
