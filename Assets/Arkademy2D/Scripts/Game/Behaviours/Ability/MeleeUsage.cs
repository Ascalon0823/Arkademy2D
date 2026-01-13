using System;
using System.Linq;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours.Ability
{
    public class MeleeUsage : Usage
    {
        public override void Init(Usable usable)
        {
            base.Init(usable);
            var candidate = usable.GetTargets()
                .FirstOrDefault();
            if (candidate)
            {
                candidate.TakeDamage(1000);
            }
            Destroy(gameObject);
        }
    }
}