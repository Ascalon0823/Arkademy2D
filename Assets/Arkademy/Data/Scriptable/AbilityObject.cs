using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Data.Scriptable
{
    
    public abstract class AbilityObject : ScriptableObject
    {
        public Ability ability;
    }

    public abstract class AbilityObject<TEffect> : AbilityObject where TEffect : Ability.Effect
    {
        public List<TEffect> effects = new List<TEffect>();
    }
}