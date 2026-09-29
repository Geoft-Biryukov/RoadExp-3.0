using Microsoft.Extensions.Logging;
using RoadExp.ComputerMeasuringSystem.Data;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;

namespace RoadExp.ComputerMeasuringSystem
{
    public class ComputerSystemFactory : IMeasuringSystemsFactory<ComputerSystemSettings, ComputerDataItem>
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;

        public ComputerSystemFactory(ILoggerFactory loggerFactory)
        {            
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger<ComputerSystemFactory>();
        }

        public IMeasuringSystem<ComputerDataItem> CreateMeasuringSystem(ComputerSystemSettings settings)
        {            
            var device = CreateDevice(settings);
            return new ComputerSystem(device, settings, _logger);
        }

        private IDeviceAdapter<ComputerDataItem> CreateDeviceAdapter(ComputerSystemSettings settings)
        {
            var logger = _loggerFactory.CreateLogger<ComputerAdapter>();
            return new ComputerAdapter(settings.ElementsCount, logger);
        }

        private IDevice<ComputerDataItem> CreateDevice(ComputerSystemSettings settings)
        {
            var adapter = CreateDeviceAdapter(settings);
            var writer = new ComputerDataWriter(settings.FileName);
            var storage = new DataStorage<ComputerDataItem>(1000, writer, _logger);
            var device = new ComputerDevice(adapter, storage);

            return device;
        }
    }
}
