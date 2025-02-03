using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class WeaponAttack : AbilityBase
    {
        public override float GetRange()
        {
            return user.characterData.weapon.offense.range / 100f;
        }

        public override bool CanUse(AbilityEventData eventData)
        {
            return base.CanUse(eventData) && ValidWeapon();
        }

        public virtual bool ValidWeapon()
        {
            return user.characterData.weapon != null && user.characterData.weapon.offense != null;
        }

        public override float GetUseTime()
        {
            return useTime / (user.characterData.weapon.offense.speed / 100f);
        }
    }
}