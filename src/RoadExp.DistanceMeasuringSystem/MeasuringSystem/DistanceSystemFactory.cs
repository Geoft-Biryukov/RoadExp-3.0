using Microsoft.Extensions.Logging;
using RoadExp.DistanceMeasuringSystem.DeviceComponents;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;

namespace RoadExp.DistanceMeasuringSystem.MeasuringSystem
{
    public class DistanceSystemFactory : IMeasuringSystemsFactory<DistanceSystemSettings, DistanceDataItem>
    {
        private const int _bufferSize = 128;
        private readonly ILogger _logger;
        private readonly IDeviceAdapterFactory<DistanceSystemSettings> _adapterFactory;
        private readonly IByteArrayParser<DistanceDataItem> _parser;
        private readonly ILoggerFactory _loggerFactory;

        public DistanceSystemFactory(
            IDeviceAdapterFactory<DistanceSystemSettings> adapterFactory,
            IByteArrayParser<DistanceDataItem> parser,
            ILoggerFactory loggerFactory)
        {
            _adapterFactory = adapterFactory;
            _parser = parser;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<DistanceSystemFactory>();
        }

        /// <summary>
        /// Создает измерительную систему ДПП
        /// и настраивает её в соответствии с переданными
        /// настройками
        /// </summary>
        /// <param name="settings">Настройки измерительной системы</param>
        /// <returns>Настроенная измерительная система ДПП</returns>
        public IMeasuringSystem<DistanceDataItem> CreateMeasuringSystem(DistanceSystemSettings settings)
        {
            _logger.LogInformation("Создается и настраивается система ДПП");

            var adapter = _adapterFactory.Create(settings);

            var storage = CreateDataStorage(settings);
            var device = new DistanceDevice(adapter, storage, _loggerFactory.CreateLogger<DistanceDevice>());

            return new DistanceSystem(_adapterFactory, device, settings, _parser, _loggerFactory.CreateLogger<DistanceSystem>());
        }

        private IDataStorage CreateDataStorage(DistanceSystemSettings settings)
        {
            var writer = new DataWriter(_loggerFactory.CreateLogger<DataWriter>());
            var storage = new DataStorage(_bufferSize, writer, _loggerFactory.CreateLogger<DataStorage>());

            return storage;
        }
    }
}
