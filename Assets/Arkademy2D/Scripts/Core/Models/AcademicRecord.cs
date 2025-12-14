using System.Collections.Generic;

namespace Arkademy2D.Core.Models
{
    public class AcademicRecord
    {
        public int ModulePoints;
        public Dictionary<string, int> ModuleProgress = new Dictionary<string, int>();
    }
}