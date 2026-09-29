using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.Models
{
    public class ApplicationUserSettings
    {
        public string FolderToSaveData { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "RoadExp");
    }
}
