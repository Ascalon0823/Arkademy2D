using System.Collections.Generic;
using UnityEngine;

namespace Arkademy
{
    public class ContactDamage : CollisionAction<CharacterBehaviour>
    {
        public float interval;
        public int damage;
        public bool dealDamageOnBegin = true;
        private Dictionary<CharacterBehaviour, float> _damageRecord = new();

        protected override void ContactUpdated(CharacterBehaviour other)
        {
            if (dealDamageOnBegin && !_damageRecord.ContainsKey(other))
            {
                DealDamage(other);
            }
            base.ContactUpdated(other);
            if (_contactTime[other] - _damageRecord[other] >= interval)
            {
                DealDamage(other);
            }
        }

        protected virtual void DealDamage(CharacterBehaviour other)
        {
            other.TakeDamage(damage);
            _damageRecord[other] = Time.fixedTime;
        }
    }
}