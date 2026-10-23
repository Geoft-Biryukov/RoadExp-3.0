using Microsoft.Extensions.Logging;
using RoadExp.DistanceMeasuringSystem.MeasuringSystem;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;

namespace RoadExp.DistanceMeasuringSystem
{
    public class DistanceSystem : IMeasuringSystem<DistanceDataItem>
    {
        #region private fields        
        private IDevice _device;        
        private readonly IDevicesFactory<DistanceSystemSettings> _devicesFactory;
        private readonly DistanceSystemSettings _settings;
        private readonly IByteArrayParser<DistanceDataItem> _parser;
        private readonly IFileHeaderFactory _headerFactory;
        private readonly ILogger<DistanceSystem> _logger;
        private CancellationTokenSource _listenerCts;
        private Task _listener;
        private bool _isAcquisitionStarted = false;
        #endregion

        public DistanceSystem(            
            IDevicesFactory<DistanceSystemSettings> devicesFactory,
            DistanceSystemSettings settings,
            IByteArrayParser<DistanceDataItem> parser,
            IFileHeaderFactory headerFactory,
            ILogger<DistanceSystem> logger)
        {
            _devicesFactory = devicesFactory ?? throw new ArgumentNullException(nameof(devicesFactory));           
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _parser = parser;
            _headerFactory = headerFactory ?? throw new ArgumentNullException(nameof(headerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            

            _logger.LogInformation("Система \"ДПП\" успешно создана");
        }

        #region events
        public event EventHandler<DataEventArgs<DistanceDataItem>> DataReceived;
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region properties        

        public bool IsActive { get; private set; } = false;

        public MeasuringSystemSettings Settings => _settings;
        #endregion

        #region public methods
        public async Task InitializeAsync()
        {
            if (IsActive)
            {
                _logger.LogWarning("Попытка повторной инициализации ДПП системы");
                return;
            }

            _logger.LogInformation("Выполняется инициализация ДПП системы");

            var devices = _devicesFactory.CreateDevices(_settings);

            if (devices.Count > 1)
            {
                throw new NotSupportedException("Датчик пройденного пути не поддерживает работу со многими Device'ами");
            }

            if (devices == null || devices.Count == 0)
            {
                _logger.LogError("Не удалось создать Device в ДПП системе");
                ErrorOccurred?.Invoke(this, new("Не удалось инициализировать систему Датчик пройденного пути"));
                return;
            }

            _device = devices.First();

            await _device.InitializeAsync();
            _device.ErrorOccurred += DeviceErrorOccurred;

            IsActive = true;

            _listenerCts = new CancellationTokenSource();

            _listener = DataChannelListener(_listenerCts.Token);

            _logger.LogInformation("Инициализация ДПП системы успешно выполнена");
        }

        public void Deinitialize()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка деинициализации ДПП системы, инициализация которой не выполнялась");
                return;
            }

            _logger.LogInformation("Выполняется деинициализация ДПП системы");

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

            _logger.LogInformation("Деинициализация ДПП системы успешно выполнена");
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

            var fileHeader = _headerFactory.CreateFileHeader(SystemIds.Distance.ToUint16(), 0);
            var fileName = Path.Combine(_settings.Path, _settings.FileName);

            var dataFileParams = new DataFileParameters(fileName, fileHeader, Array.Empty<byte>());

            _device.StartAcquisition(dataFileParams);
            _isAcquisitionStarted = true;

            _logger.LogInformation("Сбор данных ДПП системы успешно запущен");
        }

        public void StopAcquisition()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка остановить сбор данных ДПП системы, инициализация которой не выполнялась");                
                return;
            }

            if (!_isAcquisitionStarted)
            {
                _logger.LogWarning("Попытка остановить сбор данных ДПП системы, который не начинался");
                return;
            }

            _device.StopAcquisition();
            _isAcquisitionStarted = false;

            _logger.LogInformation("Сбор данных ДПП системы успешно остановлен");
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
