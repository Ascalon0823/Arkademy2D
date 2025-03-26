using System;
using UnityEngine;
using Arkademy.Data;
using Attribute = Arkademy.Data.Attribute;

namespace Arkademy.Gameplay.Ability
{
    public class RangePayload : AbilityPayload
    {
        public Projectile projectilePrefab;

        public override void Init(AbilityEventData data, AbilityBase parent, float dura, float triggerTime,
            Action<AbilityPayload> onTriggered)
        {
            base.Init(data, parent, dura, triggerTime, onTriggered);
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.Setup((int)parent.user.Attributes.GetBase(Attribute.Type.Attack),
                parent.user.faction, false
            );
        }
    }
}