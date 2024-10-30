using System;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Destructible : MonoBehaviour
    {
        public long durability;
        public Action<long, long> OnDurabilityUpdated;

        public void SetDurability(long newDurability)
        {
            if (durability == newDurability) return;
            var temp = durability;
            durability = newDurability;
            OnDurabilityUpdated?.Invoke(temp, newDurability);
        }

        private void OnDisable()
        {
            OnDurabilityUpdated = null;
        }
    }
}