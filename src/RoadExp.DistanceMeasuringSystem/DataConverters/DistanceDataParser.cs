using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;

namespace RoadExp.DistanceMeasuringSystem.DataConverters
{
    public class DistanceDataParser : IByteArrayParser<DistanceDataItem>
    {
        private readonly IByteArrayConverter<DistanceRawData> _converter;

        public DistanceDataParser(IByteArrayConverter<DistanceRawData> converter)
        {
            _converter = converter;            
        }

        public DistanceDataItem Parse(in MeasurementBatch batch)
        {
            var rawData = _converter.FromByteArray(batch.Data);

            var distanceData = new DistanceData
            {
                ImpulseCount = rawData.MainWayCounter,
            };

            return new DistanceDataItem(batch.Index, distanceData);
        }
    }
}
