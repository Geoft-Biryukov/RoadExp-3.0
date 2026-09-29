using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RoadExp.Auxiliaries.HardwareConnections;
using RoadExp.Measurements.Core;
using System.IO.Ports;

namespace RoadExp.GeometryMeasuringSystem.Tests
{
    [TestFixture]
    public class GeometrySerialPortAdapterTests
    {        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(15)]
        public async Task ProcessData_ShouldCorrectProcess(int packetCount)
        {
            // Arrange
            int packetLength = 60;

            int counter = 0;
            var indexProvider = new Mock<IIndexProvider>();

            indexProvider.Setup(p => p.GetIndex()).Returns(() => counter++);

            var logger = NullLogger<GeometrySerialPortAdapter>.Instance;

            var emptyData = CreateEmptyData(1f, packetCount, packetLength);

            var port = new SerialPortInMemory(emptyData);
            var adapter = new GeometrySerialPortAdapter(port, indexProvider.Object, logger);

            // Act Assert

            var cts = new CancellationTokenSource();

            await adapter.InitializeAsync(cts.Token);

            Assert.That(port.IsOpen, Is.True);

            var reader = adapter.DataChannel;
            var batches = new List<MeasurementBatch>();

            while (reader.TryRead(out MeasurementBatch batch))
            {
                batches.Add(batch);
            }

            Assert.That(batches, Has.Count.EqualTo(packetCount));

            counter = 0;

            foreach (var batch in batches)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(batch.Index, Is.EqualTo(counter));
                    Assert.That(batch.Data, Is.Not.Null);
                    Assert.That(batch.Data, Has.Length.EqualTo(packetLength));
                    Assert.That(batch.Data, Is.EqualTo(emptyData.Skip(counter * packetLength).Take(packetLength).ToArray()));
                });

                counter++;
            }

            adapter.Deinitialize();

            Assert.That(port.IsClosed, Is.True);

            adapter.Dispose();


            Assert.That(port.IsDisposed, Is.True);
        }

        private byte[] CreateEmptyData(float startPacketNumber, int packetCount, int packetLength)
        {
            
            byte[] buffer = new byte[packetCount * packetLength];

            for (int i = 0; i < packetCount; i++)
            {
                byte[] packet = new byte[packetLength];

                packet[0] = 0xFF;
                packet[1] = 0x01;
                packet[2] = 0x13;

                byte[] numberBytes = BitConverter.GetBytes((float)(startPacketNumber + i));

                Buffer.BlockCopy(numberBytes, 0, packet, 8, numberBytes.Length);

                Buffer.BlockCopy(packet, 0, buffer, i * packetLength, packetLength);
            }
            return buffer;
        }
    }

    internal class SerialPortInMemory : IHardwareConnector
    {
        private readonly MemoryStream _stream;
        private bool _isOpen = false;   
        private bool _isDisposed = false;
        private bool _isClosed = false;

        public SerialPortInMemory(byte[] source)
        {            
            _stream = new MemoryStream(source);
        }

        public string PortName { get; set; }
        public int BaudRate { get; set; }

        public Stream BaseStream => _stream;        

        public bool IsOpen => _isOpen;
        
        public event SerialDataReceivedEventHandler DataReceived;
        public event SerialErrorReceivedEventHandler ErrorReceived;
        
        public void Open()
        {
            _isOpen = true;
        }        

        public void Close()
        {
            _stream.Close();
            _isClosed = true;
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _isDisposed = true;
        }

        public bool IsDisposed => _isDisposed;

        public bool IsClosed => _isClosed;
    }
}