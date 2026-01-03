using UnityEngine;

namespace Arkademy2D.Game.Damage
{
    public class Trigger : MonoBehaviour
    {
        public int damage;
        public int faction;
        private void OnTriggerEnter2D(Collider2D other)
        {
            var health = other.GetComponent<Behaviours.Character>();
            if (!health || health.faction == faction) return;
            health.TakeDamage(damage);
        }
    }
}