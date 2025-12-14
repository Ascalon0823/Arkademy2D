using System;
using System.Linq;
using Arkademy2D.Core.Data.Static.Academic;
using Arkademy2D.Core.Models;

namespace Arkademy2D.Core.System
{
    public static class AcademicSystem
    {
        public static bool ModuleAvailable(ModuleData moduleData, AcademicRecord academicRecord)
        {
            if (!moduleData) throw new NullReferenceException();
            if (moduleData.prerequisites == null || moduleData.prerequisites.Count == 0) return true;
            return moduleData.prerequisites.All(x =>
                academicRecord.ModuleProgress.TryGetValue(x.Id, out var progression) && progression >= 100);
        }
    }
}