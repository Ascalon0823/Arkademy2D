using System.Collections.Generic;

namespace Arkademy2D.Core.Models
{
    public class ModuleData
    {
        public static readonly List<ModuleData> Modules = new()
        {
            new ModuleData { DisplayName = "Basic spell" },
            new ModuleData { DisplayName = "Intermediate spell", Prerequisites = new List<string> { "Basic spell" } },
        };

        public string DisplayName;
        public List<string> Prerequisites = new List<string>();
    }
}