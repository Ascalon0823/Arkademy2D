using System;
using Arkademy.Data;
using UnityEngine;
using Attribute = Arkademy.Data.Attribute;
using Random = UnityEngine.Random;

namespace Arkademy.Gameplay.Ability
{
    public class MeleePayload : AbilityPayload
    {
        public Animator animator;
        public Trigger trigger;
        public int remainingTriggerCount;
        public int currentHitCount;
        public int triggerCount;
        public int damagePercent;
        public int maxDamagePercent;
        public int maxTriggerCount;
        public int hitCount;
        public int maxHitCount;

        public override void Init(AbilityEventData data, AbilityBase parent, float dura,
            Action<AbilityPayload> onTriggered)
        {
            base.Init(data, parent, dura, onTriggered);
            
            remainingTriggerCount = Mathf.FloorToInt(Mathf.Lerp(triggerCount, maxTriggerCount, 1f / 20f));
            currentHitCount = Mathf.FloorToInt(Mathf.Lerp(hitCount, maxHitCount, 1f / 20f));
            transform.localScale = new Vector3(parent.GetRange(), parent.GetRange());
            animator.speed = 1 / dura;
            trigger.OnTrigger.AddListener(c =>
            {
                if (c.GetCharacter(out var chara) && chara.faction != parent.user.faction && triggerCount > 0)
                {
                    var damages = new long[currentHitCount];
                    for (var i = 0; i < currentHitCount; i++)
                    {
                        var baseDamage = parent.user.Attributes.GetBase(Attribute.Type.Attack);
                        baseDamage = baseDamage *
                            Mathf.FloorToInt(Mathf.Lerp(damagePercent, maxDamagePercent, 1f / 20f)) / 100;
                        baseDamage = Random.Range(80, 120) * baseDamage / 100;
                        damages[i] = baseDamage;
                    }

                    chara.TakeDamage(new DamageData(damages));

                    trigger.Ignores.Add(c);
                    chara.KnockBack(transform.up.normalized * 0.25f);
                    remainingTriggerCount--;
                }
            });
            trigger.trigger.enabled = false;
            OnTriggered.AddListener((a) => { trigger.trigger.enabled = true; });
        }
    }
}