using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class MeleeBaseAttack : AbilityBase
    {
        public MeleePayload meleePayload;
        public override float GetRange()
        {
            return user.Attributes.Get(Attribute.Type.Range);
        }

        public override float GetUseTime()          
        {
            return 1f/user.Attributes.Get(Attribute.Type.AttackSpeed);
        }

        public override void Use(AbilityEventData eventData, bool canceled = false)
        {
            base.Use(eventData, canceled);
            var payload = Instantiate(meleePayload, transform);
            payload.transform.localPosition = Vector3.zero;
            var direction = eventData.TryGetDirection(user.transform.position, out var dir) ? dir : user.facing;
            payload.transform.up = direction;
            payload.transform.localScale = new Vector3(GetRange(), GetRange());
            payload.remainingLife = GetUseTime();
            payload.triggerPoint = payload.triggerPointPercentage * payload.remainingLife;
            payload.trigger.OnTrigger.AddListener(c =>
            {
                if (c.GetCharacter(out var chara) && chara.faction != user.faction && payload.triggerCount>0)
                {
                    chara.TakeDamage(new DamageData(user.Attributes.GetBase(Attribute.Type.Attack)));
                    chara.KnockBack(direction.normalized * 1f);
                    payload.trigger.Ignores.Add(c);
                    payload.triggerCount--;
                }
            });
        }
    }
}