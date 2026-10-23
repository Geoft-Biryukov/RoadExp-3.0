using HidSharp;
using Microsoft.Extensions.Logging;
using RoadExp.DistanceMeasuringSystem.DataConverters;
using RoadExp.DistanceMeasuringSystem.MeasuringSystem;
using RoadExp.Measurements.Core;

namespace RoadExp.DistanceMeasuringSystem.DeviceComponents
{
    public class DistanceDeviceAdapterFactory : IDeviceAdapterFactory<DistanceSystemSettings>
    {
        private const string errorMessage = "Датчик пройденного пути: ошибка подключения";

        private readonly IIndexProvider _indexProvider;
        private readonly IByteArrayConverter<DistanceRawData> _byteArrayConverter;
        private readonly ILoggerFactory _loggerFactory;



        public DistanceDeviceAdapterFactory(IIndexProvider indexProvider, IByteArrayConverter<DistanceRawData> byteArrayConverter, ILoggerFactory loggerFactory)
        {
            _indexProvider = indexProvider;
            _byteArrayConverter = byteArrayConverter;
            _loggerFactory = loggerFactory;
        }

        public IDeviceAdapter Create(DistanceSystemSettings settings)
        {
            if (settings.IsEmulationMode)
            {
                return new DistanceEmulationAdapter(
                    _indexProvider,
                    _byteArrayConverter,
                    _loggerFactory.CreateLogger<DistanceEmulationAdapter>());
            }
            else
            {
                var loader = DeviceList.Local;

                var hidDevice = loader
                    .GetDevices(DeviceTypes.Hid, d => DeviceFilterHelper.MatchHidDevices(d, 1003, 8221, null, null))
                    .Cast<HidDevice>()
                    .FirstOrDefault();
                
                int inputBufferSize = 17;

                HidStream? stream = null;
                bool? isOpened = hidDevice?.TryOpen(stream: out stream);

                if (!isOpened.HasValue || !isOpened.Value || stream == null)
                {
                    return null;
                }

                var logger = _loggerFactory.CreateLogger<DistanceDeviceAdapter>();

                return new DistanceDeviceAdapter(
                    stream,
                    inputBufferSize,
                    _indexProvider,
                    _byteArrayConverter,
                    logger);
            }
        
        }        
    }
}
