using Newtonsoft.Json;
using RoadExp.Measurements.Core;

namespace RoadExp.ComputerMeasuringSystem
{
    public class ComputerSystemSettingsManager : IMeasuringSystemSettingsManager<ComputerSystemSettings>
    {
        private readonly ComputerSystemSettings _defaultSettings = new ComputerSystemSettings
        {
            FileName = "computer-system.dat",
            IsEmulationMode = true,
            ElementsCount = 1,            
        };
        
        public ComputerSystemSettings Load(string fileName)
        {
            JsonSerializer serializer = new JsonSerializer();

            using (var reader = new StreamReader(File.OpenRead(fileName)))
            {
                var jsonTextReader = new JsonTextReader(reader);

                return serializer.Deserialize<ComputerSystemSettings>(jsonTextReader);                
            }
        }

        public bool Save(ComputerSystemSettings settings, string fileName)
        {
            JsonSerializer serializer = new();

            using (var writer = new StreamWriter(File.OpenWrite(fileName)))
            {
                var jsonTextWriter = new JsonTextWriter(writer);

                serializer.Serialize(jsonTextWriter, settings);
            }

            return true;
        }

        public ComputerSystemSettings GetDefaultSettings()
        {
            return _defaultSettings;
        }
       
        public ComputerSystemSettings LoadOrDefault(string fileName)
        {
            if (File.Exists(fileName))
            {
                var settings = Load(fileName);
            
                if (settings != null)
                    return settings;              
            }

            return _defaultSettings;
        }       
    }
}
