using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.SystemsConnection;
using System.Threading.Channels;

namespace RoadExp.VideoRecordingSystem.DeviceComponents
{
    public class VideoEmulationDeviceAdapter : IDeviceAdapter
    {
        #region private fields
        private const int _channelCapacity = 1024;

        private readonly IIndexProvider _indexProvider;
        private readonly ITriggerProvider _triggerProvider;
        private readonly IVideoFrameProvider _frameProvider;

        private Channel<MeasurementBatch> _channel;
        #endregion

        #region constructor
        public VideoEmulationDeviceAdapter(
            IIndexProvider indexProvider, 
            ITriggerProvider triggerProvider, 
            IVideoFrameProvider frameProvider)
        {
            _indexProvider = indexProvider ?? throw new ArgumentNullException(nameof(indexProvider));
            _triggerProvider = triggerProvider ?? throw new ArgumentNullException(nameof(triggerProvider));
            _frameProvider = frameProvider ?? throw new ArgumentNullException(nameof(frameProvider));            
        }
        #endregion

        #region events
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region properties
        public ChannelReader<MeasurementBatch> DataChannel => _channel;
        #endregion

        #region public methods
        public Task InitializeAsync(CancellationToken token)
        {
            _channel = CreateConfiguredChannel(_channelCapacity);
            _triggerProvider.Triggered += OnTriggerReceived;

            return Task.CompletedTask;
        }

        public void Deinitialize()
        {
            _triggerProvider.Triggered -= OnTriggerReceived;
            _channel?.Writer.TryComplete();            
        }

        public void Dispose()
        {
            Deinitialize();
        }
        #endregion

        #region private methods
        private static Channel<MeasurementBatch> CreateConfiguredChannel(int channelCapacity)
        {
            var options = new BoundedChannelOptions(channelCapacity)
            {
                SingleReader = true,
                SingleWriter = true,
                FullMode = BoundedChannelFullMode.DropOldest,
            };

            return Channel.CreateBounded<MeasurementBatch>(options);
        }

        private void OnTriggerReceived(object? sender, EventArgs e)
        {
            byte[] frame = _frameProvider.GetVideoFrame();
            long index = _indexProvider.GetIndex();

            var batch = new MeasurementBatch(index, frame);

            _channel.Writer.TryWrite(batch);
        }
        #endregion
    }
}
