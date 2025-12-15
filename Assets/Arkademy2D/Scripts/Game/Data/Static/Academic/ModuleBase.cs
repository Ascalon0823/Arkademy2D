using System.Collections.Generic;

namespace Arkademy2D.Game.Data.Static.Academic
{
    public class ModuleBase : StaticData<ModuleBase>
    {
        public string Id => id.ToString();
        public string displayName;
        public List<ModuleBase> prerequisites = new List<ModuleBase>();
    }
}