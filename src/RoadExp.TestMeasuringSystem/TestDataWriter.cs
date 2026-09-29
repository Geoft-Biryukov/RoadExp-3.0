using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Data.TestMeasuringSystem;

namespace TestMeasuringSystem
{
    public class TestDataWriter : IDataWriter<TestDataItem>, IDisposable
    {
        private StreamWriter _streamWriter;

        public TestDataWriter(string fileName)
        {
            _streamWriter = File.CreateText(fileName);
        }       

        public void Write(TestDataItem value)
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
