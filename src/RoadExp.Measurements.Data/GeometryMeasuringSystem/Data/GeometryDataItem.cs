using RoadExp.Measurements.Core;

namespace RoadExp.Measurements.Data.GeometryMeasuringSystem.Data
{
    public class GeometryDataItem : IDataItem<GeometryData>
    {
        public GeometryDataItem(long index, GeometryData value)
        {
            Index = index;
            Value = value;
        }

        public long Index { get; }

        public GeometryData Value { get; }
    }
}
