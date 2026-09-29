using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.TestTriggeredSystem;

namespace RoadExp.TestTriggeredSystem
{
    public class TestTriggeredFactory : IMeasuringSystemsFactory<TestTriggeredSettings, TestTriggeredDataItem>
    {
        private readonly ILogger _logger;

        public TestTriggeredFactory(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IMeasuringSystem<TestTriggeredDataItem> CreateMeasuringSystem(TestTriggeredSettings settings)
        {
            var device = CreateDevice(settings);
            return new TestTriggeredSystem(device, settings);
        }

        private IDeviceAdapter<TestTriggeredDataItem> CreateDeviceAdapter(TestTriggeredSettings settings)
        {
            return new TestTriggeredAdapter();
        }

        private IDevice<TestTriggeredDataItem> CreateDevice(TestTriggeredSettings settings)
        {
            var adapter = CreateDeviceAdapter(settings);
            var writer = new TestTriggeredWriter(settings.FileName);
            var storage = new DataStorage<TestTriggeredDataItem>(1000, writer, _logger);
            var device = new TestTriggeredDevice(adapter, storage);

            return device;
        }
    }
}
