using RoadExp.Measurements.Core;

namespace RoadExp.Measurements.Common.Tests.Data
{
    public class IntData : IDataItem<int>, IEquatable<IntData>
    {
        public IntData(long index, int value)
        {
            Index = index;
            Value = value;
        }

        public long Index { get; }

        public int Value { get; }

        public bool Equals(IntData? other)
        {
            if (other == null) return false;

            return other.Index == Index && other.Value == Value;
        }
    }
}
