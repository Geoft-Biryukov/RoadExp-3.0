using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;


namespace RoadExp.GpsMeasuringSystem
{
    /// <summary>
    /// Измерительная система gps
    /// </summary>
    public class GpsSystem : IMeasuringSystem<GpsDataItem>
    {
        #region private fields        
        private IDevice _device;
        private readonly IDevicesFactory<GpsSystemSettings> _devicesFactory;
        private readonly GpsSystemSettings _settings;
        private readonly IByteArrayParser<GpsDataItem> _parser;
        private readonly IFileHeaderFactory _headerFactory;
        private readonly ILogger<GpsSystem> _logger;
        private CancellationTokenSource _listenerCts;
        private Task _listener;
        private bool _isAcquisitionStarted = false;
        #endregion

        #region constructor
        public GpsSystem(
            IDevicesFactory<GpsSystemSettings> devicesFactory,
            GpsSystemSettings settings,
            IByteArrayParser<GpsDataItem> parser,
            IFileHeaderFactory headerFactory,
            ILogger<GpsSystem> logger)
        {            
            _devicesFactory = devicesFactory ?? throw new ArgumentNullException(nameof(devicesFactory));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _headerFactory = headerFactory ?? throw new ArgumentNullException(nameof(headerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            

            _logger.GpsInformation("Gps-система успешно создана");
        }

        #endregion

        #region events        
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        public event EventHandler<DataEventArgs<GpsDataItem>> DataReceived;
        #endregion

        #region properties       

        public bool IsActive { get; private set; } = false;

        public MeasuringSystemSettings Settings => _settings;
        #endregion

        #region public methods
        public async Task InitializeAsync()
        {
            if(IsActive)
            {
                _logger.LogWarning("Попытка повторной инициализации GPS системы");
                return;
            }

            _logger.LogInformation("Выполняется инициализация GPS системы");

            var devices = _devicesFactory.CreateDevices(_settings);

            if (devices.Count > 1)
            {
                throw new NotSupportedException("GPS-система не поддерживает работу со многими Device'ами");
            }

            if (devices == null || devices.Count == 0)
            {
                _logger.LogError("Не удалось создать Device в GPS-системе");
                ErrorOccurred?.Invoke(this, new("Не удалось инициализировать систему GPS"));
                return;
            }

            _device = devices.First();
            _device.ErrorOccurred += DeviceErrorOccurred;

            await _device.InitializeAsync();
            IsActive = true;

            _listenerCts = new CancellationTokenSource();

            _listener = DataChannelListener(_listenerCts.Token);

            _logger.GpsInformation("Инициализация Gps-системы успешно выполнена");
        }

        public void Deinitialize()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка деинициализации GPS-системы, инициализация которой не выполнялась");
                return;
            }

            _logger.LogInformation("Выполняется деинициализация GPS-системы");

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

            _logger.LogInformation("Деинициализация GPS-системы успешно выполнена");
        }

        public void Dispose()
        {
            Deinitialize();

            if(_device != null) 
                Task.Run(async () => await _device.DisposeAsync());
        }
       
        public void StartAcquisition()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка начать сбор данных ДПП системы, инициализация которой не выполнялась");
                return;
            }

            if (_isAcquisitionStarted)
            {
                _logger.LogWarning("Повторная попытка начать сбор данных ДПП системы");
                return;
            }

            var fileHeader = _headerFactory.CreateFileHeader(SystemIds.Gps.ToUint16(), 0);
            var fileName = Path.Combine(_settings.Path, _settings.FileName);

            var dataFileInfo = new DataFileParameters(fileName, fileHeader, Array.Empty<byte>());

            _device.StartAcquisition(dataFileInfo);
            _isAcquisitionStarted = true;

            _logger.LogInformation("Сбор данных GPS-системы успешно запущен");
        }

        public void StopAcquisition()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка остановить сбор данных GPS-системы, инициализация которой не выполнялась");
                return;
            }

            if (!_isAcquisitionStarted)
            {
                _logger.LogWarning("Попытка остановить сбор данных GPS-системы, который не начинался");
                return;
            }

            _device.StopAcquisition();
            _isAcquisitionStarted = false;

            _logger.LogInformation("Сбор данных GPS-системы успешно остановлен");
        }       

        #endregion

        #region private methods       
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
