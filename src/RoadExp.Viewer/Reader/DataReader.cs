using FluentResults;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RoadExp.Viewer.Reader
{    
    /// <summary>
    /// Чтение зарегистрированных данных из бинарного файла RoadExp
    /// </summary>
    public class DataReader
    {
        #region private fields
        private readonly int _fileHeaderSize = Marshal.SizeOf<CommonFileHeader>();
        private readonly int _packetHeaderSize = Marshal.SizeOf<PacketHeader>();
        #endregion

        #region public methods       

        /// <summary>
        /// Асинхронное чтение бинарных данных
        /// </summary>
        public async Task<Result<MeasurementFileData>> ReadDataAsync(string fileName)
        {
            CommonFileHeader header = default;
            SortedList<long, MeasurementBatch> data = new SortedList<long, MeasurementBatch>();
            byte[] metadata;

            // 1. Открываем поток с флагом асинхронности
            using (var stream = new FileStream(
                fileName,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true)) // Включаем асинхронный I/O
            {
                // 2. Асинхронное чтение заголовка файла
                var headerBytes = new byte[_fileHeaderSize];
                if (await ReadExactBytesAsync(stream, headerBytes) < _fileHeaderSize)
                    return Result.Fail("Не удалось прочитать заголовок файла");

                header = StructConverter.ByteArrayToStruct<CommonFileHeader>(headerBytes);

                if (!CheckHeader(header))
                    return Result.Fail("Получен некорректный заголовок файла");

                // 3. Асинхронное чтение метаданных
                if (header.MetadataSize > 0)
                {
                    metadata = new byte[header.MetadataSize];
                    if (await ReadExactBytesAsync(stream, metadata) < header.MetadataSize)
                        return Result.Fail("Некорректные метаданные измерительной системы");
                }
                else
                {
                    metadata = Array.Empty<byte>();
                }

                // 4. Асинхронное чтение пакетов данных
                var packetHeaderBytes = new byte[_packetHeaderSize];

                while (stream.Position < stream.Length)
                {
                    // Проверяем, осталось ли в файле достаточно байт для заголовка пакета
                    if (stream.Length - stream.Position < _packetHeaderSize)
                        break;

                    if (await ReadExactBytesAsync(stream, packetHeaderBytes) < _packetHeaderSize)
                        continue;

                    var packetHeader = StructConverter.ByteArrayToStruct<PacketHeader>(packetHeaderBytes);

                    if (!CheckPacketHeader(packetHeader))
                        continue;

                    long index = packetHeader.Index;
                    int length = packetHeader.PayloadLength;

                    // Чтение полезной нагрузки (Payload)
                    var payload = new byte[length];
                    if (await ReadExactBytesAsync(stream, payload) < length)
                        continue;

                    data.Add(index, new MeasurementBatch(index, payload));
                }
            }

            var fileData = new MeasurementFileData(header, metadata, data);
            return Result.Ok(fileData);
        }

        #endregion

        #region private methods

        /// <summary>
        /// Вспомогательный метод для гарантированного чтения точного количества байт асинхронно
        /// </summary>
        private static async Task<int> ReadExactBytesAsync(Stream stream, Memory<byte> buffer)
        {
            int totalBytesRead = 0;
            while (totalBytesRead < buffer.Length)
            {
                int bytesRead = await stream.ReadAsync(buffer[totalBytesRead..]);
                if (bytesRead == 0) // Конец файла достигнут раньше времени
                    break;
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }

        private static bool CheckHeader(in CommonFileHeader header)
        {
            string code = Encoding.ASCII.GetString(header.UniqueCode);
            return code.Equals("ROAD");
        }

        private static bool CheckPacketHeader(in PacketHeader packetHeader)
        {
            return packetHeader.SyncWord == 0xAABB;
        }
        #endregion
    }
}
