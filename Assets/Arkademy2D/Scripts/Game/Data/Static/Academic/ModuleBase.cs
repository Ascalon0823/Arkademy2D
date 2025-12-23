using System.Collections.Generic;

namespace Arkademy2D.Game.Data.Static.Academic
{
    public class ModuleBase : StaticData<ModuleBase>
    {
        public string Id => id.ToString();
        public List<ModuleBase> prerequisites = new List<ModuleBase>();
    }
}