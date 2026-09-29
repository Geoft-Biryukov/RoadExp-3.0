using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.TestMeasuringSystem;

namespace TestMeasuringSystem
{
    public class TestMeasuringSystemFactory : IMeasuringSystemsFactory<TestMeasuringSystemSettings, TestDataItem>
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;

        public TestMeasuringSystemFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger<TestMeasuringSystemFactory>();
        }

        public IMeasuringSystem<TestDataItem> CreateMeasuringSystem(TestMeasuringSystemSettings settings)
        {
            var device = CreateDevice(settings);
            return new TestSystem(device, settings);
        }

        private IDeviceAdapter<TestDataItem> CreateDeviceAdapter(TestMeasuringSystemSettings settings)
        {
            return new TestAdapter(settings.Period, settings.Value);
        }

        private IDevice<TestDataItem> CreateDevice(TestMeasuringSystemSettings settings)
        {
            var adapter = CreateDeviceAdapter(settings);
            var writer = new TestDataWriter(settings.FileName);
            var storage = new DataStorage<TestDataItem>(1000, writer, _logger);
            var device = new TestDevice(adapter, storage);

            return device;
        }       
    }
}
