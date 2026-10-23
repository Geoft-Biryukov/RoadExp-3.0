using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using System.Threading.Channels;

namespace RoadExp.Measurements.Common
{
    /// <summary>
    /// Универсальное хранилище данных. Управляет жизненным циклом файла и фоновым конвейером.    
    /// </summary>    
    public sealed class DataStorage : IDataStorage
    {
        #region Private Fields
        private readonly Channel<MeasurementBatch> _channel;
        private readonly IDataWriter _writer;
        private readonly ILogger _logger;
        private readonly Task _worker;

        private readonly SemaphoreSlim _controlLock = new SemaphoreSlim(1, 1);

        private volatile bool _savingEnabled;
        private int _disposed; // 0 = false, 1 = true
        private const int _bufferCapacity = 128;
        #endregion

        #region Constructor
        public DataStorage(IDataWriter writer, ILogger<DataStorage> logger)
        {           
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var options = new BoundedChannelOptions(_bufferCapacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,                   // Читатель ВСЕГДА один (наш фоновый воркер)
                SingleWriter = false,                  // Писать в Add могут разные потоки (например, из Device)
            };

            _channel = Channel.CreateBounded<MeasurementBatch>(options);
            _worker = ProcessQueueAsync();
        }
        #endregion

        #region Public Methods       
        /// <summary>
        /// Потокобезопасно открывает новый файл. Защищено семафором от параллельного закрытия устройства.
        /// </summary>
        public void OpenFile(DataFileParameters dataFileParams)
        {
            ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) == 1, this);

            // Быстрая проверка флага без захвата тяжелого семафора
            if (_savingEnabled)
                throw new InvalidOperationException("Попытка поменять имя файла при включенной записи.");

            _controlLock.Wait();
            try
            {
                // Повторная проверка под локом для исключения Race Condition
                if (_savingEnabled)
                    throw new InvalidOperationException("Попытка поменять имя файла при включенной записи.");

                _logger.LogInformation("Устанавливаем новое имя файла: {FileName}", dataFileParams.FileName);

                _writer.OpenFile(dataFileParams.FileName, dataFileParams.FileHeader, dataFileParams.Metadata);
            }
            finally
            {
                _controlLock.Release();
            }
        }

        public void StartSaving()
        {
            _savingEnabled = true;
        }

        public void StopSaving()
        {
            _savingEnabled = false;
            _writer.FinalizeFile();
            _writer.Flush();
        }

        /// <summary>
        /// ГОРЯЧИЙ ПУТЬ (Hot Path). Вызывается с высокой частотой.
        /// Абсолютно Lock-free / Semaphore-free логика.
        /// </summary>
        public void Add(in MeasurementBatch batch)
        {
            if (Volatile.Read(ref _disposed) == 1 || !_savingEnabled)
                return;

            _channel.Writer.TryWrite(batch);
        }

        /// <summary>
        /// Потокобезопасное и гарантированное завершение работы конвейера без дедлоков.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
                return;

            await _controlLock.WaitAsync().ConfigureAwait(false);
            try
            {
                _savingEnabled = false;
                _channel.Writer.TryComplete();

                try
                {
                    await _worker.ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при завершении фонового воркера DataStorage");
                }
                finally
                {
                    _writer?.Dispose();
                }
            }
            finally
            {
                _controlLock.Release();
                _controlLock.Dispose();
            }

            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Фоновый воркер, который изолирует работу с DataWriter в ОДНОМ потоке.
        /// </summary>
        private async Task ProcessQueueAsync()
        {
            try
            {
                // Метод ReadAllAsync() гарантирует последовательное чтение строго в один поток выполнения
                await foreach (var item in _channel.Reader.ReadAllAsync().ConfigureAwait(false))
                {
                    try
                    {
                        _writer.Write(item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при записи элемента с индексом {Index} в IDataWriter", item.Index);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критический сбой самого фонового канала чтения DataStorage.");
                throw;
            }
        }

        #endregion
    }

}
