using RoadExp.ComputerMeasuringSystem.Data;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using System.Text;

namespace RoadExp.ComputerMeasuringSystem
{
    public class ComputerDataWriter : IDataWriter<ComputerDataItem>
    {
        private StreamWriter _streamWriter;
        private bool _wasHeaderWritten = false;

        public ComputerDataWriter(string fileName)
        {
            _streamWriter = File.CreateText(fileName);
        }

        public void Write(ComputerDataItem value)
        {
            if(!_wasHeaderWritten)
            {
                var headerString = GetHeaderString(value);
                _streamWriter.WriteLine(headerString);
                _wasHeaderWritten = true;
            }

            var dataString = GetDataString(value);
            _streamWriter.WriteLine(dataString);
            _streamWriter.Flush();
        }
        
        private string GetHeaderString(ComputerDataItem value)
        {
            var header = value.Value.Header;

            var builder = new StringBuilder();

            builder.AppendLine($"Computer name = {header.MachineName}");
            builder.AppendLine($"User name = {header.UserName}");
            builder.AppendLine($"Operating System = {header.OsVersion}");
            builder.AppendLine($"Processor count = {header.ProcessorCount}");
            builder.AppendLine($"Domain = {header.UserDomain}");

            return builder.ToString();
        }

        private string GetDataString(ComputerDataItem value)
        {
            var time = TimeSpan.FromTicks(value.Index);
            var performance = value.Value.Performance;

            var builder = new StringBuilder();

            builder.Append($"{value.Index}: ");
            builder.Append($"Processor = {performance.CpuUsage}%; ");
            builder.Append($"Memory = {performance.AvailableRam}(Mb)");

            return builder.ToString();
        }

        public void OpenFile(string fileName, CommonFileHeader fileHeader, byte[] metadata)
        {
            _streamWriter?.Dispose();
            _streamWriter = File.CreateText(fileName);
        }

        public void Dispose()
        {
            _streamWriter.Dispose();
        }

        public void Flush()
        {
            _streamWriter.Flush();
        }
    }
}
