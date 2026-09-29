using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.HardwareConnections;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using System.IO.Ports;

namespace RoadExp.GpsMeasuringSystem
{
    public class GpsDeviceAdapterFactory : IDeviceAdapterFactory<GpsSystemSettings>
    {
        private const int _dataBits = 8;
        private readonly IIndexProvider _indexProvider;
        private readonly IByteArrayConverter<GpsRawData> _converter;
        private readonly ILoggerFactory _loggerFactory;

        public GpsDeviceAdapterFactory(
            IIndexProvider indexProvider,
            IByteArrayConverter<GpsRawData> converter,
            ILoggerFactory loggerFactory)
        {
            _indexProvider = indexProvider ?? throw new ArgumentNullException(nameof(indexProvider));
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public IDeviceAdapter Create(GpsSystemSettings settings)
        {
            if (settings.IsEmulationMode)
            {
                return new GpsSimulationAdapter(_indexProvider, _converter, _loggerFactory.CreateLogger<GpsSimulationAdapter>());
            }
            else
            {
                IDeviceAdapter retVal = null!;
                try
                {                    
                    var serialPort = new SerialPort(settings.PortName, settings.BaudRate, Parity.None, _dataBits)
                    {
                        ReadTimeout = 5000,
                        WriteTimeout = 5000
                    };

                    var connector = new SerialPortWrapper(serialPort);

                    retVal = new GpsSerialPortAdapter(
                        connector,
                        _indexProvider,
                        _loggerFactory.CreateLogger<GpsSerialPortAdapter>());
                }
                catch (Exception e)
                {
                    if (e is ArgumentException || e is IOException)
                    {
                        retVal = new ProxyDeviceAdapter("GPS-система: ошибка подключения");
                    }
                    else
                    {
                        throw;
                    }
                }

                return retVal;
            }
        }
    }
}
