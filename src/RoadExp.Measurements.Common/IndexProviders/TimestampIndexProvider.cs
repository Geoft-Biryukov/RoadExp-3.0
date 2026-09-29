using RoadExp.Measurements.Core;
using System.Diagnostics;

namespace RoadExp.Measurements.Common.IndexProviders
{
    public class TimestampIndexProvider : IIndexProvider
    {
        public long GetIndex() => Stopwatch.GetTimestamp();
        
    }
}
