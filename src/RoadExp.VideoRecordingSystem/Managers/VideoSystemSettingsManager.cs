using RoadExp.Measurements.Common;
using RoadExp.VideoRecordingSystem.MeasuringSystem;

namespace RoadExp.VideoRecordingSystem.Managers
{
    public class VideoSystemSettingsManager : MeasuringSystemSettingsManagerBase<VideoSystemSettings>
    {
        private readonly VideoSystemSettings _defaultSettings = new VideoSystemSettings
        {
            FileName = "video.road",
            IsEmulationMode = true,
        };
        public override VideoSystemSettings GetDefaultSettings()
        {
            return _defaultSettings;
        }
    }
}
