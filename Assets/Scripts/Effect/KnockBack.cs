using System;
using UnityEngine;

namespace Arkademy.Effect
{
    [Serializable]
    public class KnockBack : Effect
    {
        public Vector2 velocity;
        public float power;

        public override Effect Dup()
        {
            return new KnockBack
            {
                target = target,
                giver = giver,
                duration = duration,
                power = power,
                velocity = velocity,
                updated = updated,
                updateImmediate = updateImmediate
            };
        }

        public override void Update()
        {
            if (!updated)
            {
                velocity = (target.transform.position - giver.transform.position).normalized * Time.deltaTime * power;
            }

            if (target) target.velocityOverride = velocity;
            base.Update();
        }

        public override void Ended()
        {
            base.Ended();
            if (target) target.velocityOverride = null;
        }

        public override bool CanApplyTo(CharacterBehaviour target)
        {
            return true;
        }
    }
}