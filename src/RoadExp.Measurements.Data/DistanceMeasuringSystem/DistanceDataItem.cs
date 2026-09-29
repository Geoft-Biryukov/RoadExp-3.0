using RoadExp.Measurements.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadExp.Measurements.Data.DistanceMeasuringSystem
{
    public class DistanceDataItem : IDataItem<DistanceData>
    {
        public DistanceDataItem(long index, DistanceData value)
        {
            Index = index;
            Value = value;
        }

        public long Index { get; }

        public DistanceData Value {get; }
    }
}
