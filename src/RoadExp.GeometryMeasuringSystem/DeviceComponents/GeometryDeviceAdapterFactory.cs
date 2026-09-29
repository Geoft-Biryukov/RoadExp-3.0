using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.HardwareConnections;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using System.IO.Ports;

namespace RoadExp.GeometryMeasuringSystem
{
    public class GeometryDeviceAdapterFactory : IDeviceAdapterFactory<GeometrySystemSettings>
    {
        private readonly IIndexProvider _indexProvider;
        private readonly ILoggerFactory _loggerFactory;

        public GeometryDeviceAdapterFactory(IIndexProvider indexProvider, ILoggerFactory loggerFactory)
        {
            _indexProvider = indexProvider ?? throw new ArgumentNullException(nameof(indexProvider));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public IDeviceAdapter Create(GeometrySystemSettings settings)
        {
            if (settings.IsEmulationMode)
            {
                return new GeometryEmulationAdapter(_indexProvider, _loggerFactory.CreateLogger<GeometryEmulationAdapter>());
            }
            else
            {
                IDeviceAdapter adapter = null!;

                try
                {
                    var serialPort = new SerialPort(settings.PortName, settings.BaudRate)
                    {
                        Parity = Parity.None,
                        StopBits = StopBits.One,
                        DataBits = 8,
                        ReadTimeout = 5000,
                        WriteTimeout = 5000
                    };

                    var wrapper = new SerialPortWrapper(serialPort);
                    
                    adapter = new GeometrySerialPortAdapter(
                        wrapper,
                        _indexProvider, 
                        _loggerFactory.CreateLogger<GeometrySerialPortAdapter>());
                }
                catch (Exception e)
                {
                    if(e is ArgumentException || e is IOException)
                    {
                        adapter = new ProxyDeviceAdapter("Геометрия: ошибка подключения");
                    }
                }
                return adapter;
            }

        }
    }

}
