using System;
using Arkademy.Data;
using UnityEngine;
using Attribute = Arkademy.Data.Attribute;
using Random = UnityEngine.Random;

namespace Arkademy.Gameplay.Ability
{
    public class ChargedProjectilePayload : AbilityPayload
    {
        public float maxChargeTime;
        public float chargedTime;
        public Projectile projectilePrefab;
        public int damagePercent;
        public int maxDamagePercent;
        public int hitCount;
        public int maxHitCount;
        public int triggerCount;
        public int maxTriggerCount;
        public int remainingTriggerCount;
        public int currentHitCount;
        public Vector2 dir;

        public GameObject indicator;

        public override void Init(AbilityEventData data, AbilityBase parent, float dura, Action<AbilityPayload> onTriggered)
        {
            base.Init(data, parent, dura, onTriggered);
            indicator.SetActive(false);
        }

        public override void UpdatePayload(AbilityEventData data, bool canceled)
        {
            base.UpdatePayload(data, canceled);
            
            if (canceled)
            {
                indicator.SetActive(false);
                remainingTriggerCount = Mathf.FloorToInt(Mathf.Lerp(triggerCount, maxTriggerCount, ability.GetLevel() / 20f));
                currentHitCount = Mathf.FloorToInt(Mathf.Lerp(hitCount, maxHitCount, ability.GetLevel() / 20f));
                var projectile = Instantiate(projectilePrefab,transform.position,transform.rotation);
                projectile.dir = dir;
                projectile.OnHit += c =>
                {
                    if (c.GetCharacter(out var chara) && chara.faction != ability.user.faction && triggerCount > 0)
                    {
                        var damages = new long[currentHitCount];
                        for (var i = 0; i < currentHitCount; i++)
                        {
                            var baseDamage = ability.user.Attributes.GetBase(Attribute.Type.Attack);
                            baseDamage = baseDamage *
                                Mathf.FloorToInt(Mathf.Lerp(damagePercent, maxDamagePercent, ability.GetLevel() / 20f)) / 100;
                            baseDamage = Random.Range(80, 120) * baseDamage / 100;
                            damages[i] = baseDamage;
                        }

                        chara.TakeDamage(new DamageData(damages));
                        projectile.ignores.Add(c);
                        chara.KnockBack(projectile.dir.normalized * 0.25f);
                        remainingTriggerCount--;
                    }
                };
                return;
            }
            dir = (data.TryGetDirection(transform.position, out var d) && d.sqrMagnitude>0 ? d : ability.user.facing).normalized;
            dir = dir.sqrMagnitude == 0 ? Vector2.up : dir;
            Debug.DrawLine(transform.position, transform.position + (Vector3)dir * 10f, Color.red);
            chargedTime += Time.deltaTime;
            indicator.transform.up = dir;
            indicator.SetActive(true);
        }
    }
}