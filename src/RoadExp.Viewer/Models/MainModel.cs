using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;
using RoadExp.Viewer.Reader;
using System.Collections.Generic;

namespace RoadExp.Viewer.Models
{
    public class MainModel
    {
        #region private fields
        private readonly IByteArrayParser<DistanceDataItem> _distanceParser;
        private readonly IByteArrayParser<GpsDataItem> _gpsParser;
        private readonly IByteArrayParser<GeometryDataItem> _geometryParser;
        #endregion

        #region constructor
        public MainModel(
            IByteArrayParser<DistanceDataItem> distanceParser,
            IByteArrayParser<GpsDataItem> gpsParser,
            IByteArrayParser<GeometryDataItem> geometryParser)
        {
            _distanceParser = distanceParser ?? throw new System.ArgumentNullException(nameof(distanceParser));
            _gpsParser = gpsParser ?? throw new System.ArgumentNullException(nameof(gpsParser));
            _geometryParser = geometryParser ?? throw new System.ArgumentNullException(nameof(geometryParser));
        }
        #endregion

        #region public methods
        public void SetFileData(string path, MeasurementFileData data)
        {
            if (data.FileHeader.SystemId == 1)
            {
                DistanceData = ProcessData(path, data, _distanceParser);
            }

            if (data.FileHeader.SystemId == 2)
            {
                GpsData = ProcessData(path, data, _gpsParser);
            }

            if (data.FileHeader.SystemId == 3)
            {
                GeometryData = ProcessData(path, data, _geometryParser);
            }
        }
        #endregion

        #region properties
        public MeasuredData<DistanceDataItem>? DistanceData { get; private set; }
        public MeasuredData<GpsDataItem>? GpsData { get; private set; }
        public MeasuredData<GeometryDataItem>? GeometryData { get; private set; }
        #endregion

        #region private methods       

        private static MeasuredData<T> ProcessData<T>(string path, MeasurementFileData data, IByteArrayParser<T> parser) where T : IDataItem 
        {            
            var result = new SortedList<long, T>();

            foreach (var batch in data.Batches)
            {
                long index = batch.Value.Index;
                var item = parser.Parse(batch.Value);

                result.Add(index, item);
            }

            return new MeasuredData<T>(path, data, result);
        }
        #endregion
    }
}
