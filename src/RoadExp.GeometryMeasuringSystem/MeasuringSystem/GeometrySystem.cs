using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;

namespace RoadExp.GeometryMeasuringSystem.MeasuringSystem
{
    public class GeometrySystem : IMeasuringSystem<GeometryDataItem>
    {
        #region private fields        
        private IDevice _device;
        private readonly IDevicesFactory<GeometrySystemSettings> _devicesFactory;
        private readonly GeometrySystemSettings _settings;
        private readonly IByteArrayParser<GeometryDataItem> _parser;
        private readonly IFileHeaderFactory _headerFactory;
        private readonly ILogger<GeometrySystem> _logger;
        private CancellationTokenSource _listenerCts;
        private Task _listener;
        private bool _isAcquisitionStarted = false;
        #endregion

        public GeometrySystem(
            IDevicesFactory<GeometrySystemSettings> devicesFactory,            
            GeometrySystemSettings settings,
            IByteArrayParser<GeometryDataItem> parser,
            IFileHeaderFactory headerFactory,
            ILogger<GeometrySystem> logger)
        {
            _devicesFactory = devicesFactory ?? throw new ArgumentNullException(nameof(devicesFactory));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _headerFactory = headerFactory ?? throw new ArgumentNullException(nameof(headerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            

            _logger.LogInformation("Система Геометрия успешно создана");
        }

        #region events
        public event EventHandler<DataEventArgs<GeometryDataItem>> DataReceived;
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
                _logger.LogWarning("Попытка повторной инициализации системы Геометрия");
                return;
            }

            _logger.LogInformation("Выполняется инициализация системы Геометрия");

            var devices = _devicesFactory.CreateDevices(_settings);

            if (devices.Count > 1)
            {
                throw new NotSupportedException("Система Геометрия не поддерживает работу со многими Device'ами");
            }

            if (devices == null || devices.Count == 0)
            {
                _logger.LogError("Не удалось создать Device в системе Геометрия");
                ErrorOccurred?.Invoke(this, new("Не удалось инициализировать систему Датчик пройденного пути"));
                return;
            }

            _device = devices.First();

            await _device.InitializeAsync();
            _device.ErrorOccurred += DeviceErrorOccurred;

            IsActive = true;

            _listenerCts = new CancellationTokenSource();

            _listener = DataChannelListener(_listenerCts.Token);

            _logger.LogInformation("Инициализация системы Геометрия успешно выполнена");
        }

        public void Deinitialize()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка деинициализации системы Геометрия, инициализация которой не выполнялась");
                return;
            }

            _logger.LogInformation("Выполняется деинициализация системы Геометрия");

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

            _logger.LogInformation("Деинициализация системы Геометрия успешно выполнена");
        }
        public void Dispose()
        {
            Deinitialize();

            if(_device  != null) 
                Task.Run(async () => await _device.DisposeAsync());
        }
       
        public void StartAcquisition()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка начать сбор данных системы Геометрия, инициализация которой не выполнялась");
                return;
            }

            if (_isAcquisitionStarted)
            {
                _logger.LogWarning("Повторная попытка начать сбор данных системы Геометрия");
                return;
            }

            var fileHeader = _headerFactory.CreateFileHeader(SystemIds.Geometry.ToUint16(), 0);
            var fileName = Path.Combine(_settings.Path, _settings.FileName);

            var dataFileParams = new DataFileParameters(fileName, fileHeader, Array.Empty<byte>());

            _device.StartAcquisition(dataFileParams);
            _isAcquisitionStarted = true;

            _logger.LogInformation("Сбор данных системы Геометрия успешно запущен");
        }

        public void StopAcquisition()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка остановить сбор данных системы Геометрия, инициализация которой не выполнялась");
                return;
            }

            if (!_isAcquisitionStarted)
            {
                _logger.LogWarning("Попытка остановить сбор данных системы Геометрия, который не начинался");
                return;
            }

            _device.StopAcquisition();
            _isAcquisitionStarted = false;

            _logger.LogInformation("Сбор данных системы Геометрия успешно остановлен");
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
