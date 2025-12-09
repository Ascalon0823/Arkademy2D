using System;
using Arkademy2D.Game.Actors;
using UnityEngine;

namespace Arkademy2D.Game.Damage
{
    public class Trigger : MonoBehaviour
    {
        public int damage;
        public int faction;
        private void OnTriggerEnter2D(Collider2D other)
        {
            var health = other.GetComponent<Health>();
            if (!health || health.faction == faction) return;
            health.current -=  damage;
        }
    }
}