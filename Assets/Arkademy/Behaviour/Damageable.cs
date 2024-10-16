using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Damageable : MonoBehaviour
    {
        public int faction;
        public CircleCollider2D trigger;
        public float damageEffectiveness;
        public Action<Data.DamageEvent> OnDamageEvent;

        public void TakeDamage(Data.DamageEvent damage)
        {
            damage.damage = Mathf.CeilToInt(damage.damage * (damageEffectiveness / 10000f));
            OnDamageEvent?.Invoke(damage);
        }

        private void OnDisable()
        {
            OnDamageEvent = null;
        }
    }
}