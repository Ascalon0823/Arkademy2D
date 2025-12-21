using System.Collections.Generic;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Usable
{
    public class UsableBase : StaticData<UsableBase>
    {
        public string Id => id.ToString();
        public float useTime;
        public List<UsableEffectDefinition> usableEffects;
        public bool HasEffect => usableEffects?.Count > 0;
    }
}