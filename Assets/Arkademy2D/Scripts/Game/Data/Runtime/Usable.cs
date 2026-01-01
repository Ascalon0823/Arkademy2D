using System;
using System.Collections.Generic;
using Arkademy2D.Game.Data.Static.Usable;
using UnityEngine;

namespace Arkademy2D.Game.Data.Runtime
{
    [Serializable]
    public class Usable
    {
        public Usage activeUsage;
        [SerializeField] protected UsableEffectDefinition definition;
        public List<Attribute> attributes;
        public Usable(UsableEffectDefinition definition)
        {
            this.definition = definition;
        }
    
        public virtual bool CanUse(UseContext ctx)
        {
            return true;
        }
    }
}