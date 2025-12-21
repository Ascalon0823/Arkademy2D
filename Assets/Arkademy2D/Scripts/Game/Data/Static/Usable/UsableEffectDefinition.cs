using Arkademy2D.Game.Data.Runtime;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Usable
{
    public abstract class UsableEffectDefinition : ScriptableObject
    {
        public abstract void UseEffect(UseContext context);
    }
}