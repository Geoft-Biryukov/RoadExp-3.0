using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;
using System.Collections.Generic;

namespace RoadExp.Viewer.Reader
{
    public class MeasurementFileData
    {
        public MeasurementFileData(CommonFileHeader fileHeader, byte[] metadata, SortedList<long, MeasurementBatch> batches)
        {
            FileHeader = fileHeader;
            Metadata = metadata;
            Batches = batches;
        }

        public CommonFileHeader FileHeader { get; }
        public byte[] Metadata { get; }
        public SortedList<long, MeasurementBatch> Batches { get; }
        public bool IsComplete { get; set; }
    }
}
