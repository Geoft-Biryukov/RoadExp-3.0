using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.SystemsConnection;
using RoadExp.VideoRecordingSystem.DeviceComponents.Emulation;
using RoadExp.VideoRecordingSystem.MeasuringSystem;

namespace RoadExp.VideoRecordingSystem.DeviceComponents
{
    public class VideoDeviceAdapterFactory : IDeviceAdapterFactory<VideoSystemSettings>
    {
        #region private fields
        private readonly IIndexProvider _indexProvider;
        private readonly ITriggerProvider _triggerProvider;
        private readonly IEmulatedFrameProviderFactory _emulatedFrameProviderFactory;
        private readonly ILoggerFactory _loggerFactory;
        #endregion

        #region constructor
        public VideoDeviceAdapterFactory(
            IIndexProvider indexProvider, 
            ITriggerProvider triggerProvider, 
            IEmulatedFrameProviderFactory emulatedFrameProviderFactory,
            ILoggerFactory loggerFactory)
        {
            _indexProvider = indexProvider ?? throw new ArgumentNullException(nameof(indexProvider));
            _triggerProvider = triggerProvider ?? throw new ArgumentNullException(nameof(triggerProvider));
            _emulatedFrameProviderFactory = emulatedFrameProviderFactory ?? throw new ArgumentNullException(nameof(emulatedFrameProviderFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        #endregion

        #region public methods
        public IDeviceAdapter Create(VideoSystemSettings settings)
        {
            if (settings.IsEmulationMode)
            {
                var frameProvider = _emulatedFrameProviderFactory.CreateNext();

                return new VideoEmulationDeviceAdapter(_indexProvider, _triggerProvider, frameProvider);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
