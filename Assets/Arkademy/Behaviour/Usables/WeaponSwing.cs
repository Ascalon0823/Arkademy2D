using System.Collections;
using System.Linq;
using UnityEngine;

namespace Arkademy.Behaviour.Usables
{
    public class WeaponSwing : Usable
    {
        public Equipment equipment;

        public float delayPercentage;
        public int[] damagePercentages;

        public override bool Use()
        {
            if (!base.Use()) return false;
            var cd = equipment.data.attributes.FirstOrDefault(x => x.key == "Base Speed");
            nextUseTime = 1f / (cd.value / 100f);
            StartCoroutine(InternalUse());
            return true;
        }

        IEnumerator InternalUse()
        {
            yield return new WaitForSeconds(delayPercentage * nextUseTime);
            var pos = (Vector2)equipment.transform.position + equipment.facingDir.normalized;
            var size = Vector2.one * 2f;
            var colliders = Physics2D.OverlapBoxAll(pos, size,
                Vector2.Angle(Vector2.up, equipment.facingDir), LayerMask.GetMask("Hitbox"));
            Debug.DrawLine(equipment.transform.position, pos, Color.red, 1f);
            var damage = equipment.data.attributes.FirstOrDefault(x => x.key == "Physical Attack");
            foreach (var c in colliders)
            {
                var damageable = c.GetComponent<Damageable>();
                if (!damageable || damageable.faction == user.faction) continue;
                var damageEvent = new Data.DamageEvent
                {
                    damages = new int [damagePercentages.Length]
                };
                for (var i = 0; i < damagePercentages.Length; i++)
                {
                    damageEvent.damages[i] =
                        Mathf.FloorToInt(Random.Range(90, 101) / 100f * damage.value * damagePercentages[i] / 100f);
                }

                damageable.TakeDamage(damageEvent);
            }
        }
    }
}