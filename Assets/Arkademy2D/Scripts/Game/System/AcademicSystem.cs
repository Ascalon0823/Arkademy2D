using System;
using System.Linq;
using Arkademy2D.Game.Data.Static.Academic;
using Arkademy2D.Core.Models;

namespace Arkademy2D.Game.System
{
    public static class AcademicSystem
    {
        public static bool ModuleAvailable(ModuleBase moduleBase, AcademicRecord academicRecord)
        {
            if (!moduleBase) throw new NullReferenceException();
            if (moduleBase.prerequisites == null || moduleBase.prerequisites.Count == 0) return true;
            return moduleBase.prerequisites.All(x =>
                academicRecord.ModuleProgress.TryGetValue(x.Id, out var progression) && progression >= 100);
        }
    }
}