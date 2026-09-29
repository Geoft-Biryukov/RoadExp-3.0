using Microsoft.Extensions.Logging;
using RoadExp.DistanceMeasuringSystem.MeasuringSystem;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;
using System.Text;

namespace RoadExp.DistanceMeasuringSystem
{
    public class DistanceSystem : IMeasuringSystem<DistanceDataItem>
    {
        #region private fields
        private readonly IDeviceAdapterFactory<DistanceSystemSettings> _adapterFactory;
        private readonly IDevice _device;
        private readonly DistanceSystemSettings _settings;
        private readonly IByteArrayParser<DistanceDataItem> _parser;
        private readonly ILogger<DistanceSystem> _logger;
        private CancellationTokenSource _listenerCts;
        private Task _listener;
        #endregion

        public DistanceSystem(
            IDeviceAdapterFactory<DistanceSystemSettings> adapterFactory,
            IDevice device,
            DistanceSystemSettings settings,
            IByteArrayParser<DistanceDataItem> parser,
            ILogger<DistanceSystem> logger)
        {
            _adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _parser = parser;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _device.ErrorOccurred += DeviceErrorOccurred;

            _logger.LogInformation("Система \"ДПП\" успешно создана");
        }

        #region events
        public event EventHandler<DataEventArgs<DistanceDataItem>> DataReceived;
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region properties
        public DistanceDataItem HotValue { get; private set; }

        public bool IsActive { get; private set; } = false;

        public MeasuringSystemSettings Settings => _settings;
        #endregion

        #region public methods
        public async Task InitializeAsync()
        {
            if (IsActive) return;

            await _device.InitializeAsync();
            IsActive = true;

            _listenerCts = new CancellationTokenSource();

            _listener = DataChannelListener(_listenerCts.Token);
        }

        public void Deinitialize()
        {
            if (!IsActive) return;

            if (_listenerCts != null)
            {
                _listenerCts.Cancel();

                try
                {
                    if (_listener != null)
                    {
                        _listener.GetAwaiter().GetResult();
                    }
                }
                catch (OperationCanceledException) { /* Игнорируем штатную отмену */ }
                finally
                {
                    _listenerCts.Dispose();
                    _listenerCts = null;
                    _listener = null;
                }

            }

            _device?.Deinitialize();
            IsActive = false;
        }
        public void Dispose()
        {
            Deinitialize();
            Task.Run(async () => await _device.DisposeAsync());
        }

        public void OpenFile()
        {
            var fileHeader = CreateFileHeader(_settings);
            var fileName = Path.Combine(_settings.Path, _settings.FileName);
            _device.OpenFile(fileName, fileHeader, Array.Empty<byte>());
        }

        public void StartAcquisition()
        {
            _device.StartAcquisition();
        }

        public void StopAcquisition()
        {
            _device.StopAcquisition();
        }

        public void UpdateAdapter()
        {
            var adapter = _adapterFactory.Create(_settings);
            _device.UpdateAdapter(adapter);
        }
        #endregion

        #region private methods
        private CommonFileHeader CreateFileHeader(DistanceSystemSettings settings)
        {
            return new CommonFileHeader
            {
                UniqueCode = Encoding.ASCII.GetBytes("ROAD"),
                Version = 1,
                TimeUtc = DateTime.UtcNow.Ticks,
                SystemId = 1,
                MetadataSize = 0,
            };
        }

        private async Task DataChannelListener(CancellationToken token)
        {
            await foreach (var item in _device.DataChannel.ReadAllAsync(token).ConfigureAwait(false))
            {
                DeviceDataProcess(item);
            }
        }

        private void DeviceDataProcess(in MeasurementBatch batch)
        {
            var data = _parser.Parse(batch);

            HotValue = data;

            var eventArgs = new DataEventArgs<DistanceDataItem>(data);

            DataReceived?.Invoke(this, eventArgs);
        }

        private void DeviceErrorOccurred(object? sender, MeasurementErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
        #endregion
    }
}
