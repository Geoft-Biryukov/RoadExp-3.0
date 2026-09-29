using RoadExp.Measurements.Core;
using RoadExp.Viewer.Reader;
using System.Collections.Generic;

namespace RoadExp.Viewer.Models
{
    public class MeasuredData<T> where T : IDataItem
    {
        public MeasuredData(string path, MeasurementFileData rawData, SortedList<long, T> processedData)
        {
            Path = path;
            RawData = rawData;
            ProcessedData = processedData;
        }

        public string Path { get; }
        public MeasurementFileData RawData { get; }
        public SortedList<long, T> ProcessedData { get; }
    }
}
