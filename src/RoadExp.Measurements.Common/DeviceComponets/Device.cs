using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using System.Threading.Channels;

namespace RoadExp.Measurements.Common
{
    /// <summary>
    /// Потокобезопасный класс "устройство" для управления сбором данных с поддержкой Zero-allocation конвейера.
    /// </summary>
    public abstract class Device : IDevice
    {
        #region Private Fields
        private const int _channelCapacity = 1024;

        private IDeviceAdapter _adapter;
        private readonly IDataStorage _storage;
        private readonly ILogger<Device> _logger;

        private readonly Channel<MeasurementBatch> _channel;

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private CancellationTokenSource? _cts;
        private Task? _worker;

        private bool _isInitialized = false;
        private int _disposed = 0; // 0 = false, 1 = true
        #endregion

        #region Constructor
        protected Device(IDeviceAdapter adapter, IDataStorage storage, ILogger<Device> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var options = new BoundedChannelOptions(_channelCapacity)
            {
                SingleReader = true,
                SingleWriter = true,
                FullMode = BoundedChannelFullMode.DropOldest
            };

            _channel = Channel.CreateBounded<MeasurementBatch>(options);

            _adapter.ErrorOccurred += OnErrorOccurred;
        }
        #endregion

        #region Events        
        public event EventHandler<MeasurementErrorEventArgs>? ErrorOccurred;
        #endregion

        #region Properties              
        public ChannelReader<MeasurementBatch> DataChannel => _channel.Reader;
        #endregion

        #region Public Methods       
        public virtual async Task InitializeAsync()
        {
            await _lock.WaitAsync().ConfigureAwait(false);
            try
            {
                if (_isInitialized)
                    return;

                ThrowIfDisposed();

                // Пересоздаем CTS для возможности повторного перезапуска устройства
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await _adapter.InitializeAsync(_cts.Token).ConfigureAwait(false);

                _isInitialized = true;

                // Запускаем фоновый обработчик данных
                _worker = StartProcessingAsync(_cts.Token);
            }
            catch
            {
                _isInitialized = false;
                throw;
            }
            finally
            {
                _lock.Release();
            }
        }

        public void CancelInitialization()
        {
            // Отмена безопасна и не требует захвата тяжелого семафора
            _cts?.Cancel();
        }

        public virtual void Deinitialize()
        {
            _lock.Wait();
            try
            {
                if (!_isInitialized)
                    return;

                // 1. Сигнализируем фоновому потоку о необходимости завершения
                _cts?.Cancel();

                // 2. Деинициализируем адаптер физического устройства
                _adapter.Deinitialize();

                // 3. Дожидаемся завершения фонового воркера (синхронно, с блокировкой во избежание Race Condition)
                try
                {
                    _worker?.GetAwaiter().GetResult();
                }
                catch (OperationCanceledException) { /* Игнорируем штатную отмену */ }
                finally
                {
                    _worker = null;
                }

                _isInitialized = false;
            }
            finally
            {
                _lock.Release();
            }
        }

        public void OpenFile(string fileName, CommonFileHeader fileHeader, byte[] metadata)
        {
            ThrowIfDisposed();
            _storage.OpenFile(fileName, fileHeader, metadata);
        }

        public virtual void StartAcquisition()
        {
            ThrowIfDisposed();
            _storage.StartSaving();
        }

        public virtual void StopAcquisition()
        {
            ThrowIfDisposed();
            _storage.StopSaving();
        }

        public void UpdateAdapter(IDeviceAdapter adapter)
        {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));

            _lock.Wait();
            try
            {
                ThrowIfDisposed();

                // 1. Отписываемся и деинициализируем старый адаптер
                _adapter.ErrorOccurred -= OnErrorOccurred;

                if (_isInitialized)
                {
                    _cts?.Cancel();
                    _adapter.Deinitialize();
                    try
                    {
                        _worker?.GetAwaiter().GetResult();
                    }
                    catch (OperationCanceledException) { }
                    _worker = null;
                    _isInitialized = false;
                }

                // Освобождаем ресурсы старого адаптера во избежание утечек памяти
                _adapter.Dispose();

                // 2. Устанавливаем новый адаптер
                _adapter = adapter;
                _adapter.ErrorOccurred += OnErrorOccurred;
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// Потокобезопасное асинхронное освобождение ресурсов устройства.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
                return;

            _cts?.Cancel();
            _adapter.ErrorOccurred -= OnErrorOccurred;
            _adapter.Deinitialize();
            if (_worker != null)
            {
                try
                {
                    await _worker.ConfigureAwait(false);
                }
                catch (Exception) { _logger.LogError("Исключение в Device при освобождении данных"); }
            }

            _cts?.Dispose();
            _adapter.Dispose();
            _lock.Dispose();

            await _storage.DisposeAsync().ConfigureAwait(false);

            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Methods
        private async Task StartProcessingAsync(CancellationToken token)
        {
            try
            {
                // Асинхронно читаем структуры из канала без аллокаций.            
                await foreach (MeasurementBatch batch in _adapter.DataChannel.ReadAllAsync(token).ConfigureAwait(false))
                {
                    _storage.Add(batch);
                    _channel.Writer.TryWrite(batch);
                }
            }
            catch (OperationCanceledException)
            {
                // Штатный выход из цикла по токену отмены, обработка не требуется
            }
            catch (Exception ex)
            {
                OnErrorOccurred(this, new MeasurementErrorEventArgs("Ошибка конвейера обработки данных", ex));
            }
        }

        private void ThrowIfDisposed()
        {
            if (Volatile.Read(ref _disposed) == 1)
                throw new ObjectDisposedException(GetType().FullName);
        }
        #endregion

        #region Event Handlers       
        protected virtual void OnErrorOccurred(object? sender, MeasurementErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
        #endregion
    }
}
