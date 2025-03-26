using System;
using Arkademy.Data;
using UnityEngine;
using Attribute = Arkademy.Data.Attribute;

namespace Arkademy.Gameplay.Ability
{
    public class MeleePayload : AbilityPayload
    {
        public Animator animator;
        public Trigger trigger;
        public int triggerCount;

        public override void Init(AbilityEventData data, AbilityBase parent, float dura, float triggerTime,
            Action<AbilityPayload> onTriggered)
        {
            base.Init(data, parent, dura, triggerTime, onTriggered);
            animator.speed = 1 / dura;
            
            transform.localScale = new Vector3(parent.GetRange(), parent.GetRange());
            trigger.OnTrigger.AddListener(c =>
            {
                if (c.GetCharacter(out var chara) && chara.faction != parent.user.faction && triggerCount > 0)
                {
                    chara.TakeDamage(new DamageData(parent.user.Attributes.GetBase(Attribute.Type.Attack)));
                    chara.KnockBack(transform.up.normalized * 1f);
                    trigger.Ignores.Add(c);
                    triggerCount--;
                }
            });
            trigger.trigger.enabled = false;
            OnTriggered.AddListener((a) =>
            {
                trigger.trigger.enabled = true;
            });
        }
    }
}