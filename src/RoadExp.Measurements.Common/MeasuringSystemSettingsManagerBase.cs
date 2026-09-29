using Newtonsoft.Json;
using RoadExp.Measurements.Core;

namespace RoadExp.Measurements.Common
{
    public abstract class MeasuringSystemSettingsManagerBase<T> : IMeasuringSystemSettingsManager<T> where T : MeasuringSystemSettings
    {
        public abstract T GetDefaultSettings();        

        public T Load(string fileName)
        {
            JsonSerializer serializer = new JsonSerializer();

            using (var reader = new StreamReader(File.OpenRead(fileName)))
            {
                var jsonTextReader = new JsonTextReader(reader);

                return serializer.Deserialize<T>(jsonTextReader);
            }            
        }

        public T LoadOrDefault(string fileName)
        {
            if (File.Exists(fileName))
            {
                var settings = Load(fileName);

                if (settings != null)
                    return settings;
            }

            return GetDefaultSettings();
        }

        public bool Save(T settings, string fileName)
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
