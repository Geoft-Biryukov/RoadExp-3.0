using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.Models
{
    public class SettingsModel
    {
        private const string _settingsFileName = "settings.cfg";

        public SettingsModel()
        {
            
        }

        public ApplicationUserSettings Settings { get; set; }

        public void SaveSettings()
        {
            JsonSerializer serializer = new();

            using (var writer = new StreamWriter(File.OpenWrite(_settingsFileName)))
            {
                var jsonTextWriter = new JsonTextWriter(writer);

                serializer.Serialize(jsonTextWriter, Settings);               
            }
        }

        public bool LoadSettings()
        {
            if (!File.Exists(_settingsFileName))
            {
                Settings = new ApplicationUserSettings();
                return false;
            }

            JsonSerializer serializer = new();
            
            using( var reader = new StreamReader(File.OpenRead(_settingsFileName)))
            {
                var jsonTextReader = new JsonTextReader(reader);

                Settings = serializer.Deserialize<ApplicationUserSettings>(jsonTextReader);

                if (Settings == null)
                {
                    Settings = new ApplicationUserSettings();
                    return false;
                }
            }

            return true;
            
        }
    }
}
