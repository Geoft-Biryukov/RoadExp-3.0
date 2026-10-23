using RoadExp.Measurements.Core;

namespace RoadExp.Measurements.Data.VideoRecordingSystem
{
    public class VideoDataItem : IDataItem<VideoData>
    {
        public VideoDataItem(long index, VideoData value)
        {
            Index = index;
            Value = value;
        }
        public long Index { get; }
        public VideoData Value { get; }
    }
}
