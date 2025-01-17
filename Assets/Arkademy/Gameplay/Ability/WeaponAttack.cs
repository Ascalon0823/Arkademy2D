using System.Linq;
using Arkademy.Common;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class WeaponAttack : AbilityBase
    {
        public Projectile projectile;

        public override bool CanUse(AbilityEventData eventData)
        {
            return base.CanUse(eventData) && eventData.TryGetDirection(user.transform.position, out var dir);
        }

        public override float GetCooldown()
        {
            return Calculation.CastCoolDown(user.characterData.castSpeed.value) * cooldown;
        }

        public override void Use(AbilityEventData eventData)
        {
            user.SetAttack(GetCooldown());
            if (!eventData.TryGetDirection(user.transform.position, out var dir))
            {
                dir = user.facing;
            }

            var proj = Instantiate(projectile, user.transform.position, Quaternion.identity);
            proj.dir = dir.normalized;
            proj.Setup(1000,user.faction,false);
            remainingCooldown = GetCooldown();
        }
    }
}