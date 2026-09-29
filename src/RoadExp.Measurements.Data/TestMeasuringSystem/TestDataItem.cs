using RoadExp.Measurements.Core;

namespace RoadExp.Measurements.Data.TestMeasuringSystem
{
    public class TestDataItem : IDataItem<long>
    {
        public TestDataItem(long index, long value)
        {
            Index = index;
            Value = value;
        }
        public long Index { get; }
        public long Value { get; }
    }
}
