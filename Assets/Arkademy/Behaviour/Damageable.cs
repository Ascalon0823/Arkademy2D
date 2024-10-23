using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Damageable : MonoBehaviour
    {
        public int faction;
        public CircleCollider2D trigger;
        public long damageEffectiveness;
        public Action<Data.DamageEvent> OnDamageEvent;

        public void TakeDamage(Data.DamageEvent damage)
        {
            for (var i = 0; i < damage.damages.Length; i++)
            {
                damage.damages[i] = Mathf.CeilToInt(damage.damages[i] * (damageEffectiveness / 10000f));
            }
            OnDamageEvent?.Invoke(damage);
        }

        private void OnDisable()
        {
            OnDamageEvent = null;
        }
    }
}