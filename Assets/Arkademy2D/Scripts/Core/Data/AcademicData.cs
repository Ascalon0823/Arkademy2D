using System;
using System.Collections.Generic;

namespace Arkademy2D.Core.Data
{
    public class AcademicData
    {
        public int ModulePoints;
        public Dictionary<string, int> ModuleProgress = new Dictionary<string, int>();
    }

    public class ModuleObject
    {
        public static readonly List<ModuleObject> Modules = new()
        {
            new ModuleObject { DisplayName = "Basic spell" },
            new ModuleObject { DisplayName = "Intermediate spell", Prerequisites = new List<string> { "Basic spell" } },
        };

        public string DisplayName;
        public List<string> Prerequisites = new List<string>();
    }
}