using Arkademy.Common;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class WeaponAttack : AbilityBase
    {
        public Projectile projectile;
        public override bool CanUse(Character target)
        {
            return base.CanUse(target) && target;
        }

        public override float GetCooldown()
        {
            return Calculation.CastCoolDown(user.characterData.castSpeed.value) * cooldown;
        }

        public override void Use(Character target)
        {
            base.Use(target);
            var dir = target.transform.position - user.transform.position;
            var p = Instantiate(projectile,user.transform.position,Quaternion.identity);
            p.dir = dir.normalized;
            p.faction = user.faction;
        }
    }
}