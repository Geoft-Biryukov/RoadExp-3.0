using Microsoft.Extensions.Logging.Abstractions;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Utils;
using System.Runtime.InteropServices;
using System.Text;

namespace RoadExp.Measurements.Common.Tests
{
    [TestFixture]
    public class DataWriterTests
    {
        private const string _defaultName = "data_writer_test.dat";
        private string _fileName;

        [SetUp]
        public void Setup()
        {
            var path = Path.GetTempPath();
            _fileName = Path.Combine(path, _defaultName);
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
        }

        [Test]
        public void Write_CorrectData_ShouldSaveData()
        {
            // Arrange
            var source = Enumerable
                .Range(1, 5)
                .Select(item => new MeasurementBatch(item * 345, BitConverter.GetBytes(item * 687)));

            var logger = NullLogger<DataWriter>.Instance;

            var writer = new DataWriter(logger);

            var header = new CommonFileHeader
            {
                UniqueCode = Encoding.ASCII.GetBytes("TEST"),
                Version = 1,
                SystemId = 0,
                TimeUtc = DateTime.UtcNow.Ticks,
                MetadataSize = 0
            };

            writer.OpenFile(_fileName, header, Array.Empty<byte>());

            // Act

            foreach (var item in source)
            {
                writer.Write(item);
            }

            writer.FinalizeFile();

            writer.Dispose();

            // Assert
            bool exists = File.Exists(_fileName);

            Assert.That(exists, Is.True);

            var result = ParseFile(_fileName);

            Assert.Multiple(() =>
            {
                Assert.That(result.successFlag, Is.EqualTo(0xAABBCCDD));

                Assert.That(result.header.UniqueCode, Is.EqualTo(header.UniqueCode));
                Assert.That(result.header.TimeUtc, Is.EqualTo(header.TimeUtc));
                Assert.That(result.header.Version, Is.EqualTo(header.Version));
                Assert.That(result.header.SystemId, Is.EqualTo(header.SystemId));
                Assert.That(result.header.MetadataSize, Is.EqualTo(header.MetadataSize));

                Assert.That(result.data.Select(d => d.Value.Index).ToArray(), Is.EqualTo(source.Select(s => s.Index).ToArray()));
                Assert.That(result.data.Select(d => d.Value.Data).ToArray(), Is.EqualTo(source.Select(s => s.Data).ToArray()));
            });
        }

        private static (CommonFileHeader header, KeyValuePair<PacketHeader, MeasurementBatch>[] data, uint successFlag) ParseFile(string fileName)
        {
            var memoryStream = new MemoryStream();
            var stream = File.OpenRead(fileName);

            var fileHeaderSize = Marshal.SizeOf<CommonFileHeader>();
            var packetHeaderSize = Marshal.SizeOf<PacketHeader>();

            stream.CopyTo(memoryStream);

            var bytes = memoryStream.ToArray();

            int length = bytes.Length;

            // I. Читаем флаг успешного завершения и таблицу индексов
            uint successFlag = BitConverter.ToUInt32(bytes, length - 4);

            long indexTableOffset = BitConverter.ToInt64(bytes, length - 4 - 8);

            int indexTableCount = BitConverter.ToInt32(bytes, (int)indexTableOffset);
            var indexTable = new List<long>(indexTableCount);

            for (int i = 0; i < indexTableCount; i++)
            {
                int index = (int)(indexTableOffset + 4 + i * sizeof(long));
                long current = BitConverter.ToInt64(bytes, index);
                indexTable.Add(current);
            }

            // II. Читаем заголовок файла
            var fileHeaderBytes = new byte[fileHeaderSize];
            Buffer.BlockCopy(bytes, 0, fileHeaderBytes, 0, fileHeaderSize);

            var fileHeader = StructConverter.ByteArrayToStruct<CommonFileHeader>(fileHeaderBytes);

            // III. Читаем данные
            var data = new Dictionary<PacketHeader, MeasurementBatch>();

            int pointer = fileHeaderSize;

            while (pointer < indexTableOffset)
            {
                var packetHeaderBytes = new byte[packetHeaderSize];

                Buffer.BlockCopy(bytes, pointer, packetHeaderBytes, 0, packetHeaderSize);
                var packetHeader = StructConverter.ByteArrayToStruct<PacketHeader>(packetHeaderBytes);

                pointer += packetHeaderSize;

                var payload = new byte[packetHeader.PayloadLength];
                Buffer.BlockCopy(bytes, pointer, payload, 0, payload.Length);

                pointer += packetHeader.PayloadLength;

                var value = BitConverter.ToInt32(payload);

                data.Add(packetHeader, new MeasurementBatch(packetHeader.Index, BitConverter.GetBytes(value)));
            }

            return (fileHeader, data.ToArray(), successFlag);
        }
    }
}
