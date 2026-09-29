using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;

namespace RoadExp.GpsMeasuringSystem
{
    /// <summary>
    /// Фабрика измерительной системы Gps
    /// </summary>
    public class GpsSystemFactory : IMeasuringSystemsFactory<GpsSystemSettings, GpsDataItem>
    {
        private const int _bufferSize = 128;
        private readonly IDeviceAdapterFactory<GpsSystemSettings> _adapterFactory;
        private readonly IByteArrayParser<GpsDataItem> _dataParser;
        private readonly ILoggerFactory _loggerFactory;

        public GpsSystemFactory(
            IDeviceAdapterFactory<GpsSystemSettings> adapterFactory,
            IByteArrayParser<GpsDataItem> dataParser,
            ILoggerFactory loggerFactory)
        {
            _adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
            _dataParser = dataParser ?? throw new ArgumentNullException(nameof(dataParser));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Создает измерительную систему gps
        /// и настраивает её в соответствии с переданными
        /// настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>Настроенная измерительная система gps</returns>
        public IMeasuringSystem<GpsDataItem> CreateMeasuringSystem(GpsSystemSettings settings)
        {
            var adapter = _adapterFactory.Create(settings);

            var storage = CreateDataStorage(settings);

            var device = new GpsDevice(adapter, storage, _loggerFactory.CreateLogger<GpsDevice>());

            return new GpsSystem(_adapterFactory, device, settings, _dataParser, _loggerFactory.CreateLogger<GpsSystem>());
        }

        private IDataStorage CreateDataStorage(GpsSystemSettings settings)
        {
            var writer = new DataWriter(_loggerFactory.CreateLogger<DataWriter>());
            var storage = new DataStorage(_bufferSize, writer, _loggerFactory.CreateLogger<DataStorage>());

            return storage;
        }
    }

}
