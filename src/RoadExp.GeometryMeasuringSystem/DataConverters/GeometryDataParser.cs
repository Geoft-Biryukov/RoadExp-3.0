using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;

namespace RoadExp.GeometryMeasuringSystem
{
    public class GeometryDataParser : IByteArrayParser<GeometryDataItem>
    {
        private readonly IByteArrayConverter<GeometryRawData> _converter;        

        public GeometryDataParser(IByteArrayConverter<GeometryRawData> converter)
        {
            _converter = converter;            
        }

        public GeometryDataItem Parse(in MeasurementBatch batch)
        {
            var rawData = _converter.FromByteArray(batch.Data);

            var geometryData = Helpers.ParseData(rawData.RawByteArray);

            return new GeometryDataItem(batch.Index, geometryData);
        }
    }
}
