using RoadExp.Measurements.Core;
using TestMeasuringSystem;
using Newtonsoft.Json;

namespace RoadExp.TestMeasuringSystem
{
    public class TestMeasuringSystemSettingsManager : IMeasuringSystemSettingsManager<TestMeasuringSystemSettings>
    {
        private readonly TestMeasuringSystemSettings _defaultSettings = new TestMeasuringSystemSettings
        {
            Value = 10,
            Period = 10,
            IsEmulationMode = true,
            FileName = "test-system.dat",            
        };

        public TestMeasuringSystemSettings GetDefaultSettings()
        {
            return _defaultSettings;
        }

        public TestMeasuringSystemSettings Load(string fileName)
        {
            var serializer = new JsonSerializer();

            using (var reader = new StreamReader(File.OpenRead(fileName)))
            {
                var jsonTextReader = new JsonTextReader(reader);

                return serializer.Deserialize<TestMeasuringSystemSettings>(jsonTextReader);
            }
        }

        public TestMeasuringSystemSettings LoadOrDefault(string fileName)
        {
            if (File.Exists(fileName))
            {
                var settings = Load(fileName);

                if (settings != null)
                    return settings;
            }

            return _defaultSettings;
        }

        public bool Save(TestMeasuringSystemSettings settings, string fileName)
        {
            JsonSerializer serializer = new();

            using (var writer = new StreamWriter(File.OpenWrite(fileName)))
            {
                var jsonTextWriter = new JsonTextWriter(writer);

                serializer.Serialize(jsonTextWriter, settings);
            }

            return true;
        }
    }
}
