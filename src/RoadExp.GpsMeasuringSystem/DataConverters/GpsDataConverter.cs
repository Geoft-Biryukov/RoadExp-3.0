using RoadExp.Measurements.Core;
using System.Text;

namespace RoadExp.GpsMeasuringSystem
{
    public class GpsDataConverter : IByteArrayConverter<GpsRawData>
    {
        public byte[] ToByteArray(GpsRawData data)
        {
            var gga = data.Nmea[0];
            var rmc = data.Nmea[1];

            var ggaArray = Encoding.ASCII.GetBytes(gga);
            var rmcArray = Encoding.ASCII.GetBytes(rmc);

            byte[] result = new byte[ggaArray.Length + rmcArray.Length + 2];

            result[0] = (byte)ggaArray.Length;
            result[1] = (byte)rmcArray.Length;

            Buffer.BlockCopy(ggaArray, 0, result, 2, gga.Length);
            Buffer.BlockCopy(rmcArray, 0, result, ggaArray.Length + 2, rmcArray.Length);

            return result;
        }

        public GpsRawData FromByteArray(byte[] data)
        {
            int lengthGga = data[0];
            int lengthRmc = data[1];

            var ggaString = Encoding.ASCII.GetString(data, 2, lengthGga);
            var rmcString = Encoding.ASCII.GetString(data, 2 + lengthGga, lengthRmc);

            var result = new string[] { ggaString, rmcString };

            return new GpsRawData(result);
        }
    }
}
