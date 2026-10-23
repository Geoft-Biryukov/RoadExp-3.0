using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data;
using RoadExp.Measurements.Data.VideoRecordingSystem;
using RoadExp.VideoRecordingSystem.DeviceComponents;

namespace RoadExp.VideoRecordingSystem.MeasuringSystem
{
    public class VideoSystem : IMeasuringSystem<VideoDataItem>
    {
        #region private fields
        private IDevicesFactory<VideoSystemSettings> _devicesFactory;
        private IByteArrayParser<VideoDataItem> _parser;
        private IFileHeaderFactory _headerFactory;
        private ILogger<VideoSystem> _logger;

        private readonly VideoSystemSettings _settings;
        private List<CancellationTokenSource> _listenersCts;
        private Task _listener;
        private bool _isAcquisitionStarted = false;
        private IReadOnlyList<VideoDevice> _devices;
        #endregion

        #region constructor       
        public VideoSystem(
            IDevicesFactory<VideoSystemSettings> devicesFactory, 
            VideoSystemSettings settings, 
            IByteArrayParser<VideoDataItem> parser, 
            IFileHeaderFactory headerFactory, 
            ILogger<VideoSystem> logger)
        {
            _devicesFactory = devicesFactory ?? throw new ArgumentNullException(nameof(devicesFactory));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _headerFactory = headerFactory ?? throw new ArgumentNullException(nameof(headerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region properties
        public bool IsActive { get; private set; } = false;
        public MeasuringSystemSettings Settings => _settings;
        #endregion

        #region events
        public event EventHandler<DataEventArgs<VideoDataItem>> DataReceived;
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
        #endregion

        #region public methods
        public async Task InitializeAsync()
        {
            if (IsActive)
            {
                _logger.LogWarning("Попытка повторной инициализации системы Видеосъемка");
                return;
            }

            _logger.LogInformation("Выполняется инициализация системы Видеосъемка");

            _devices = _devicesFactory.CreateDevices(_settings).Cast<VideoDevice>().ToList();

            if (_devices == null || _devices.Count == 0)
            {
                _logger.LogError("Не удалось создать Device в системе Видеосъемка");
                ErrorOccurred?.Invoke(this, new("Не удалось инициализировать систему Видеосъемка"));
                return;
            }
           
            foreach (var device in _devices) 
                await device.InitializeAsync();

            IsActive = true;

            int count = _devices.Count;

            _listenersCts = new List<CancellationTokenSource>(count);

            for (int i = 0; i < count; i++)
            {
                var cts = new CancellationTokenSource();
                _listenersCts.Add(cts);
            }

           // _listener = DataChannelListener(_listenerCts.Token);

            _logger.LogInformation("Инициализация системы Видеосъемка успешно выполнена");
        }

        public void Deinitialize()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка деинициализации системы Видеосъемка, инициализация которой не выполнялась");
                return;
            }

            _logger.LogInformation("Выполняется деинициализация системы Видеосъемка");

            if (_listenersCts != null)
            {
                 _listenersCts.ForEach(x => x.Cancel());

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
                    _listenersCts.ForEach(x =>x.Dispose());
                    _listenersCts = null;
                    _listener = null;
                }
            }

            foreach(var device in _devices) device.Deinitialize();
            IsActive = false;

            _logger.LogInformation("Деинициализация системы Видеосъемка успешно выполнена");
        }


        public void StartAcquisition()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка начать сбор данных системы Видеосъемка, инициализация которой не выполнялась");
                return;
            }

            if (_isAcquisitionStarted)
            {
                _logger.LogWarning("Повторная попытка начать сбор данных системы Видеосъемка");
                return;
            }

            var fileHeader = _headerFactory.CreateFileHeader(SystemIds.Video.ToUint16(), 0);

            for (int i = 0; i < _devices.Count; i++)
            {
                var name = Path.GetFileNameWithoutExtension(_settings.FileName);
                var ext = Path.GetExtension(_settings.FileName);

                var fileName = Path.Combine(_settings.Path, $"{name}_{_devices[i].Id}{ext}");

                var dataFileParams = new DataFileParameters(fileName, fileHeader, Array.Empty<byte>());

                _devices[i].StartAcquisition(dataFileParams);
            }
            _isAcquisitionStarted = true;

            _logger.LogInformation("Сбор данных системы Видеосъемка успешно запущен");
        }

        public void StopAcquisition()
        {
            if (!IsActive)
            {
                _logger.LogWarning("Попытка остановить сбор данных системы Видеосъемка, инициализация которой не выполнялась");
                return;
            }

            if (!_isAcquisitionStarted)
            {
                _logger.LogWarning("Попытка остановить сбор данных системы Видеосъемка, который не начинался");
                return;
            }

            foreach(var device in _devices) device.StopAcquisition();
            _isAcquisitionStarted = false;

            _logger.LogInformation("Сбор данных системы Видеосъемка успешно остановлен");
        }

        public void Dispose()
        {
            Deinitialize();

            if (_devices != null)
            {
                foreach (var device in _devices)
                {
                    device.DisposeAsync().ConfigureAwait(false).GetAwaiter();
                }
            }
        }
        #endregion

        #region private methods
        
        #endregion
    }
}
