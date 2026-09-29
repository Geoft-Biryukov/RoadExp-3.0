using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;
using System.Text;


namespace RoadExp.GpsMeasuringSystem
{
    /// <summary>
    /// Измерительная система gps
    /// </summary>
    public class GpsSystem : IMeasuringSystem<GpsDataItem>
    {
        #region private fields
        private readonly IDeviceAdapterFactory<GpsSystemSettings> _adapterFactory;
        private readonly IDevice _device;
        private readonly GpsSystemSettings _settings;
        private readonly IByteArrayParser<GpsDataItem> _parser;
        private readonly ILogger<GpsSystem> _logger;
        private CancellationTokenSource _listenerCts;
        private Task _listener;
        #endregion

        #region constructor
        public GpsSystem(
            IDeviceAdapterFactory<GpsSystemSettings> adapterFactory,
            IDevice device,
            GpsSystemSettings settings,
            IByteArrayParser<GpsDataItem> parser,
            ILogger<GpsSystem> logger)
        {
            _adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _device.ErrorOccurred += DeviceErrorOccurred;

            _logger.GpsInformation("Gps-система успешно создана");
        }

        #endregion

        #region events        
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        public event EventHandler<DataEventArgs<GpsDataItem>> DataReceived;
        #endregion

        #region properties
        public GpsDataItem HotValue { get; private set; }

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

            _logger.GpsInformation("Инициализация Gps-системы прошла успешно");
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
            _device.OpenFile(fileName, fileHeader, metadata: Array.Empty<byte>());
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
        private CommonFileHeader CreateFileHeader(GpsSystemSettings settings)
        {
            return new CommonFileHeader
            {
                UniqueCode = Encoding.ASCII.GetBytes("ROAD"),
                Version = 1,
                TimeUtc = DateTime.UtcNow.Ticks,
                SystemId = 2,
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

            var eventArgs = new DataEventArgs<GpsDataItem>(data);

            DataReceived?.Invoke(this, eventArgs);
        }


        private void DeviceErrorOccurred(object? sender, MeasurementErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
        #endregion
    }

}
