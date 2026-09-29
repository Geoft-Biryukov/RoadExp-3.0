using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using System.Text;

namespace RoadExp.GeometryMeasuringSystem
{
    public class GeometrySystem : IMeasuringSystem<GeometryDataItem>
    {
        #region private fields
        private readonly IDeviceAdapterFactory<GeometrySystemSettings> _adapterFactory;
        private readonly IDevice _device;
        private readonly GeometrySystemSettings _settings;
        private readonly IByteArrayParser<GeometryDataItem> _parser;
        private readonly ILogger<GeometrySystem> _logger;
        private CancellationTokenSource _listenerCts;
        private Task _listener;
        #endregion

        public GeometrySystem(
            IDeviceAdapterFactory<GeometrySystemSettings> adapterFactory,
            IDevice device,
            GeometrySystemSettings settings,
            IByteArrayParser<GeometryDataItem> parser,
            ILogger<GeometrySystem> logger)
        {
            _adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _parser = parser;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _device.ErrorOccurred += DeviceErrorOccurred;

            _logger.LogInformation("Система \"Геометрия\" успешно создана");
        }

        #region events
        public event EventHandler<DataEventArgs<GeometryDataItem>> DataReceived;
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region properties
        public GeometryDataItem HotValue { get; private set; }

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
        private CommonFileHeader CreateFileHeader(GeometrySystemSettings settings)
        {
            return new CommonFileHeader
            {
                UniqueCode = Encoding.ASCII.GetBytes("ROAD"),
                Version = 1,
                TimeUtc = DateTime.UtcNow.Ticks,
                SystemId = 3,
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

            var eventArgs = new DataEventArgs<GeometryDataItem>(data);

            DataReceived?.Invoke(this, eventArgs);
        }

        private void DeviceErrorOccurred(object? sender, MeasurementErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
        #endregion
    }

}
