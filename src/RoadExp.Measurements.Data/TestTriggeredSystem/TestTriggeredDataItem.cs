using RoadExp.Measurements.Core;

namespace RoadExp.Measurements.Data.TestTriggeredSystem
{
    public class TestTriggeredDataItem : IDataItem<int>
    {
        public TestTriggeredDataItem(long index, int value)
        {
            Index = index;
            Value = value;
        }

        public long Index { get; }

        public int Value { get; }
    }
}
