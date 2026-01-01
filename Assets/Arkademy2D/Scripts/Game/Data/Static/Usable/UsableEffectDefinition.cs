using System.Collections.Generic;
using Arkademy2D.Game.Data.Runtime;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Usable
{
    public abstract class UsableEffectDefinition : ScriptableObject
    {
        public abstract IReadOnlyList<AttributeBase> RequiredAttributes { get; }
        public abstract Usage GetUsage(UseContext ctx);
    }
}