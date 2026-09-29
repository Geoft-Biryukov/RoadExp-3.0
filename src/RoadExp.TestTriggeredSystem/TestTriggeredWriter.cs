using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.TestTriggeredSystem;

namespace RoadExp.TestTriggeredSystem
{
    public class TestTriggeredWriter : IDataWriter<TestTriggeredDataItem>
    {
        private StreamWriter _streamWriter;

        public TestTriggeredWriter(string fileName)
        {
            _streamWriter = File.CreateText(fileName);
        }

        public void Write(TestTriggeredDataItem value)
        {
            var time = new DateTime(value.Index);
            _streamWriter.WriteLine($"Время: {time:hh:mm:ss.ss} | Значение: {value.Value}");
            _streamWriter.Flush();
        }

        public void Dispose()
        {
            _streamWriter.Close();
            _streamWriter.Dispose();
        }

        public void OpenFile(string fileName, CommonFileHeader fileHeader, byte[] metadata)
        {
            _streamWriter?.Dispose();
            _streamWriter = File.CreateText(fileName);
        }

        public void Flush()
        {
            _streamWriter.Flush();
        }
    }
}
