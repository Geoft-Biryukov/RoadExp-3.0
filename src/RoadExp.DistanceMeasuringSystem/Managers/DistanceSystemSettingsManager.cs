using RoadExp.DistanceMeasuringSystem.MeasuringSystem;
using RoadExp.Measurements.Common;

namespace RoadExp.DistanceMeasuringSystem.Managers
{
    public class DistanceSystemSettingsManager : MeasuringSystemSettingsManagerBase<DistanceSystemSettings>
    {
        private readonly DistanceSystemSettings _defaultSettings = new DistanceSystemSettings
        {            
            FileName = "distance.road",
            IsEmulationMode = true,
        };

        public override DistanceSystemSettings GetDefaultSettings()
        {
            return _defaultSettings;
        }
    }
}
