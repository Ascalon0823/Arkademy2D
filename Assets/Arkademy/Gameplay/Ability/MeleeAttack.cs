using Arkademy.Common;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class MeleeAttack : WeaponAttack
    {
        public MeleePayload meleePrefab;

        public override bool ValidWeapon()
        {
            return base.ValidWeapon() && !user.characterData.weapon.offense.isRange;
        }
        public override void Use(AbilityEventData eventData, bool canceled = false)
        {
            base.Use(eventData,canceled);
            var current = Instantiate(meleePrefab);
            current.Init(eventData,this,GetUseTime());
            current.trigger.OnTrigger.AddListener(c =>
            {
                if (c.GetCharacter(out var chara) && chara.faction !=user.faction)
                {
                    chara.TakeDamage(Calculation.Damage(user.characterData.weapon.offense.attack));
                }
            });
        }
    }
}