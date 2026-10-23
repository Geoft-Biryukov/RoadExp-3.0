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
                var logger = _loggerFactory.CreateLogger<GpsSimulationAdapter>();
                return new GpsSimulationAdapter(_indexProvider, _converter, logger);
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

                    var logger = _loggerFactory.CreateLogger<GpsSerialPortAdapter>();

                    retVal = new GpsSerialPortAdapter(
                        connector,
                        _indexProvider,
                        logger);
                }
                catch (Exception e)
                {
                    if (e is ArgumentException || e is IOException)
                    {
                        retVal = null;
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
