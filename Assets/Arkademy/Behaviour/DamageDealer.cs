using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy.Behaviour
{
    public class DamageDealer : MonoBehaviour
    {
        [Serializable]
        public struct BeforeDamageEvent
        {
            public Collider2D target;
            public DamageDealer dealer;
        }
        public int faction;
        public Data.DamageEvent damageEventBase;
        public UnityEvent<BeforeDamageEvent> beforeDamageEvent;
        public float cooldown;
        [SerializeField] private float lastTrigger;
        public void DealDamageTo(Collider2D other)
        {
            if (!isActiveAndEnabled) return;
            if (Time.timeSinceLevelLoad - lastTrigger < cooldown) return;
            var damageable = other.GetComponent<Damageable>();
            if (!damageable) return;
            if (damageable.faction == faction) return;
            beforeDamageEvent?.Invoke(new BeforeDamageEvent
            {
                target = other,
                dealer = this
            });
            damageable.TakeDamage(damageEventBase);
            lastTrigger = Time.timeSinceLevelLoad;
        }
    }
}