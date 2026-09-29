using RoadExp.Measurements.Common;

namespace RoadExp.GeometryMeasuringSystem.Managers
{
    public class GeometrySystemSettingsManager : MeasuringSystemSettingsManagerBase<GeometrySystemSettings>
    {
        private readonly GeometrySystemSettings _defaultSettings = new GeometrySystemSettings
        {
            BaudRate = 0,
            PortName = string.Empty,
            FileName = "geometry.road",
            IsEmulationMode = true,            
        };

        public override GeometrySystemSettings GetDefaultSettings()
        {
            return _defaultSettings;
        }
    }
}
