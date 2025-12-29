using System;
using System.Collections.Generic;
using Arkademy2D.Game.Behaviours.Actor;
using UnityEngine;

namespace Arkademy2D.Game.Damage
{
    public class Contact : MonoBehaviour
    {
        public int damage;
        public int faction;
        public float frequency;
        
        private Dictionary<Health, float> lastDamage = new Dictionary<Health, float>();

        private bool ShouldDealDamage(Collider2D other, out Health health)
        {
            health = other.gameObject.GetComponent<Health>();
            return health && health.faction != faction;
        }

        private void TryDealDamage(Health health)
        {
            if (!lastDamage.TryGetValue(health, out var time) || Time.timeSinceLevelLoad - time > frequency)
            {
                health.TakeDamage(damage);
                lastDamage[health] = Time.timeSinceLevelLoad;
            }
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!ShouldDealDamage(other.collider, out var health)) return;
            TryDealDamage(health);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!ShouldDealDamage(other.collider,out var health)) return;
            TryDealDamage(health);
        }
    }
}