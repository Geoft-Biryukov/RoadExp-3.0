using RoadExp.Measurements.Core;

namespace RoadExp.ComputerMeasuringSystem.Data
{
    public class ComputerDataItem : IDataItem<ComputerData>
    {
        public ComputerDataItem(long index, ComputerData data)
        {
            Index = index;
            Value = data;
        }

        public ComputerDataItem(long index, ComputerInfoHeader header, ComputerPerformanceInfo performance)
            : this(index, new ComputerData(header, performance)) 
        { }
        
        public long Index { get; set; }

        public ComputerData Value { get; set; }
    }
}
