using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.SystemsConnection;
using RoadExp.VideoRecordingSystem.DeviceComponents.Emulation;
using RoadExp.VideoRecordingSystem.MeasuringSystem;

namespace RoadExp.VideoRecordingSystem.DeviceComponents
{
    public class VideoDevicesFactory : IDevicesFactory<VideoSystemSettings>
    {
        #region private fields
        private const int _defaultWidth = 640;
        private const int _defaultHeight = 480;
        private readonly IIndexProvider _indexProvider;
        private readonly ITriggerProvider _triggerProvider;       
        private readonly ILoggerFactory _loggerFactory;
        #endregion

        #region constructor
        public VideoDevicesFactory(
             IIndexProvider indexProvider,
             ITriggerProvider triggerProvider,             
             ILoggerFactory loggerFactory)
        {
            _indexProvider = indexProvider ?? throw new ArgumentNullException(nameof(indexProvider));
            _triggerProvider = triggerProvider ?? throw new ArgumentNullException(nameof(triggerProvider));            
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        #endregion

        #region events
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region public methods

        /// <summary>
        /// Создает список VideoDevice по заданным настройкам.
        /// Каждый Device взаимодействует со своим DataWriter
        /// </summary>
        /// <param name="settings">Настройки</param>
        /// <returns>Список сконфигурированных VideoDevice'ов</returns>
        public IReadOnlyList<IDevice> CreateDevices(VideoSystemSettings settings)
        {       
            if(settings.CameraCount == 0)
                return Enumerable.Empty<IDevice>().ToList();

            var devices = new List<IDevice>();

            var deviceLogger = _loggerFactory.CreateLogger<VideoDevice>();
            var writerLogger = _loggerFactory.CreateLogger<DataWriter>();
            var storageLogger = _loggerFactory.CreateLogger<DataStorage>();

            var adapters = CreateAdapters(settings);

            int id = 0;

            foreach (var adapter in adapters)
            {
                var writer = new DataWriter(writerLogger);
                var storage = new DataStorage(writer, storageLogger);
                var device = new VideoDevice(adapter, storage, id, deviceLogger);
                devices.Add(device);

                id++;
            }
            
            return devices;
        }
        #endregion

        #region private methods
        private IEnumerable<IDeviceAdapter> CreateAdapters(VideoSystemSettings settings)
        {
            if (settings.IsEmulationMode)
                return CreateEmulationAdapters(settings);
            else
                return CreateDeviceAdapters(settings);
        }

        private IEnumerable<IDeviceAdapter> CreateEmulationAdapters(VideoSystemSettings settings)
        {
            var adapters = new List<IDeviceAdapter>();

            int count = settings.CameraCount;

            var objects = Enum.GetValues<EmulatedObjects>();

            for (int i = 0; i < count; i++)
            {
                int index = count % objects.Length;
                var emulated = new EmulatedFrameProvider(_defaultWidth, _defaultHeight, objects[index]);
                var adapter = new VideoEmulationDeviceAdapter(_indexProvider, _triggerProvider, emulated);

                adapters.Add(adapter);
            }

            return adapters;
        }

        private IEnumerable<IDeviceAdapter> CreateDeviceAdapters(VideoSystemSettings settings)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
