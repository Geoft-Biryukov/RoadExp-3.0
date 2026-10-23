using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.VideoRecordingSystem;

namespace RoadExp.VideoRecordingSystem.DataConverters
{
    public class VideoDataParser : IByteArrayParser<VideoDataItem>
    {
        public VideoDataItem Parse(in MeasurementBatch batch)
        {
            return new VideoDataItem(batch.Index, new VideoData(batch.Data));
        }
    }
}
