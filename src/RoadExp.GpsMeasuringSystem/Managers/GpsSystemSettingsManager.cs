using RoadExp.Measurements.Common;

namespace RoadExp.GpsMeasuringSystem
{
    public class GpsSystemSettingsManager : MeasuringSystemSettingsManagerBase<GpsSystemSettings>
    {
        private readonly GpsSystemSettings _defaultSettings = new GpsSystemSettings
        {
            IsEmulationMode = true, 
            PortName = string.Empty,
            BaudRate = 0, 
            FileName = "gps.road",            
        };

        public override GpsSystemSettings GetDefaultSettings()
        {
            return _defaultSettings;
        }
    }
}
