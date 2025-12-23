using System;
using System.Collections.Generic;
using Arkademy2D.Game.Data.Static.Usable;
using UnityEngine;

namespace Arkademy2D.Game.Data.Runtime
{
    [Serializable]
    public class UsableData
    {
        public UsableBase usableBase;
        public List<UsableEffectDefinition> usableEffects = new List<UsableEffectDefinition>();
        public float remainingUseTime;
        public bool InUse => remainingUseTime > 0;

        public bool Use(UseContext context)
        {
            if (InUse) return false;
            remainingUseTime = usableBase.useTime;
            foreach (var effect in usableEffects)
            {
                effect.UseEffect(context);
            }

            return true;
        }

        public void Update(float deltaTime)
        {
            remainingUseTime-=deltaTime;
            remainingUseTime = Mathf.Max(0, remainingUseTime);
        }
    }
}