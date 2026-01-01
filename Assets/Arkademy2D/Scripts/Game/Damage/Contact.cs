using System.Collections.Generic;
using Arkademy2D.Game.Behaviours;
using UnityEngine;

namespace Arkademy2D.Game.Damage
{
    public class Contact : MonoBehaviour
    {
        public int damage;
        public int faction;
        public float frequency;
        
        private Dictionary<Character, float> lastDamage = new Dictionary<Character, float>();

        private bool ShouldDealDamage(Collider2D other, out Character health)
        {
            health = null;
            if (!enabled) return false;
            health = other.gameObject.GetComponent<Character>();
            return health && health.faction != faction;
        }

        private void TryDealDamage(Character health)
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