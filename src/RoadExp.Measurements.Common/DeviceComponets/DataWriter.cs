using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Utils;
using System.Runtime.InteropServices;

namespace RoadExp.Measurements.Common
{
    /// <summary>
    /// Универсальный класс для записи 
    /// сырых данных на диск.
    /// 
    /// Высокопроизводительный низкоуровневый писатель данных на диск.
    /// Разработан для последовательной записи из одного фонового потока (Thread-confined).
    /// 
    /// Структура файла:
    /// 
    /// CommonFileHeader
    /// Metadata
    /// PacketHeader1 payload1
    /// PacketHeader2 payload2
    /// ..................
    /// PacketHeaderN payloadN
    /// 
    /// </summary>    
    public class DataWriter : IDataWriter
    {
        #region Private Fields        
        private readonly ILogger<DataWriter> _logger;

        // Поля объявлены как volatile для безопасной передачи ссылок между 
        // управляющим потоком (OpenFile) и фоновым потоком записи (Write)
        private volatile FileStream? _stream;
        private volatile BinaryWriter? _writer;
        private int _disposed = 0;
        #endregion

        #region Constructor
        public DataWriter(ILogger<DataWriter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Открывает файл для записи и инициализирует потоки. 
        /// Вызывается перед началом измерительной сессии.
        /// </summary>
        public void OpenFile(string fileName, CommonFileHeader fileHeader, byte[] metadata)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Имя файла не может быть пустым.", nameof(fileName));

            ThrowIfDisposed();
            _logger.LogInformation("Открываем файл на запись: {FileName}", fileName);

            CloseCurrentStreamInternal();

            // Оптимальный размер буфера для высокоскоростной потоковой записи на SSD/HDD (64 КБ)
            const int bufferSize = 64 * 1024;

            // FileOptions.SequentialScan подсказывает ОС оптимизировать упреждающее чтение/запись
            _stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read, bufferSize, FileOptions.SequentialScan);
            _writer = new BinaryWriter(_stream);

            WriteFileHeaderInternal(fileHeader, metadata);
            _writer.Flush();
        }

        /// <summary>
        /// Записывает элемент данных. 
        /// МЕТОД НЕПОТОКОБЕЗОПАСЕН: должен вызываться строго из одного фонового потока воркера.
        /// </summary>
        public void Write(in MeasurementBatch batch)
        {
            var currentWriter = _writer;
            if (currentWriter == null)
                throw new InvalidOperationException("Файл не открыт. Сначала следует вызвать OpenFile.");

            var array = batch.Data;
            PacketHeader packetHeader = CreatePacketHeader(batch.Index, array.Length);

            // Zero-allocation конвертация структуры заголовка пакета в Span byte
            ReadOnlySpan<PacketHeader> headerSpan = MemoryMarshal.CreateReadOnlySpan(ref packetHeader, 1);
            ReadOnlySpan<byte> headerBytes = MemoryMarshal.AsBytes(headerSpan);

            // Прямая последовательная запись в буфер BinaryWriter
            currentWriter.Write(headerBytes);
            currentWriter.Write(array);
        }

        /// <summary>
        /// Принудительно сбрасывает буферы на диск.
        /// </summary>
        public void Flush()
        {
            _writer?.Flush();
        }

        /// <summary>
        /// Освобождает ресурсы стрима.
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
                return;

            CloseCurrentStreamInternal();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Methods
        private void CloseCurrentStreamInternal()
        {
            try
            {
                _writer?.Flush();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Не удалось выполнить Flush перед закрытием файла.");
            }

            _writer?.Dispose();
            _stream?.Dispose();

            _writer = null;
            _stream = null;
        }

        private void WriteFileHeaderInternal(CommonFileHeader fileHeader, byte[] metadata)
        {
            if (_writer == null) return;

            var fileHeaderBytes = StructConverter.StructToByteArray(fileHeader);
            _writer.Write(fileHeaderBytes);

            if (metadata != null && metadata.Length > 0)
            {
                _writer.Write(metadata);
            }
        }

        private PacketHeader CreatePacketHeader(long index, int length)
        {
            return new PacketHeader
            {
                SyncWord = 0xAABB,
                Index = index,
                PayloadLength = (ushort)length,
            };
        }

        private void ThrowIfDisposed()
        {
            if (Volatile.Read(ref _disposed) == 1)
                throw new ObjectDisposedException(GetType().FullName);
        }
        #endregion
    }
}
