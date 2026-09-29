using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GeometryMeasuringSystem;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;

namespace RoadExp.GeometryMeasuringSystem
{
    public class GeometrySystemFactory : IMeasuringSystemsFactory<GeometrySystemSettings, GeometryDataItem>
    {
        private const int _bufferSize = 128;
        private readonly ILogger _logger;
        private readonly IDeviceAdapterFactory<GeometrySystemSettings> _adapterFactory;
        private readonly IByteArrayParser<GeometryDataItem> _parser;
        private readonly ILoggerFactory _loggerFactory;

        public GeometrySystemFactory(
            IDeviceAdapterFactory<GeometrySystemSettings> adapterFactory,
            IByteArrayParser<GeometryDataItem> parser,
            ILoggerFactory loggerFactory)
        {
            _adapterFactory = adapterFactory;
            _parser = parser;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<GeometrySystemFactory>();
        }

        /// <summary>
        /// Создает измерительную систему геометрия
        /// и настраивает её в соответствии с переданными
        /// настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>Настроенная измерительная система геометрия</returns>
        public IMeasuringSystem<GeometryDataItem> CreateMeasuringSystem(GeometrySystemSettings settings)
        {
            _logger.LogInformation("Создается и настраивается система Геометрия");

            var adapter = _adapterFactory.Create(settings);

            var storage = CreateDataStorage(settings);
            var device = new GeometryDevice(adapter, storage, _loggerFactory.CreateLogger<GeometryDevice>());

            return new GeometrySystem(_adapterFactory, device, settings, _parser, _loggerFactory.CreateLogger<GeometrySystem>());
        }

        private IDataStorage CreateDataStorage(GeometrySystemSettings settings)
        {
            var writer = new DataWriter(_loggerFactory.CreateLogger<DataWriter>());
            var storage = new DataStorage(_bufferSize, writer, _loggerFactory.CreateLogger<DataStorage>());

            return storage;
        }
    }

}
