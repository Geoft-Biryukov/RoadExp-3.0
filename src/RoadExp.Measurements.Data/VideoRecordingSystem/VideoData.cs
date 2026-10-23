namespace RoadExp.Measurements.Data.VideoRecordingSystem
{
    public class VideoData
    {
        public VideoData(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; }
    }
}
