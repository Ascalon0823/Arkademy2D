using System;
using UnityEngine;
using Arkademy.Data;
using Attribute = Arkademy.Data.Attribute;

namespace Arkademy.Gameplay.Ability
{
    public class RangePayload : AbilityPayload
    {
        public Projectile projectilePrefab;

        public override void Trigger()
        {
            base.Trigger();
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.Setup((int)ability.user.Attributes.GetBase(Attribute.Type.Attack),
                ability.user.faction, false
            );
            projectile.dir = transform.up;
        }
    }
}