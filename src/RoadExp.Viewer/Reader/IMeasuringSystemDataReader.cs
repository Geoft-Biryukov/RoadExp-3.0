using RoadExp.Measurements.Core;
using System.Collections.Generic;
using System.IO;

namespace RoadExp.Viewer.Reader
{
    public interface IMeasuringSystemDataReader<T>
    {
        SortedList<long, IDataItem<T>> ReadData(Stream source);
    }
}
