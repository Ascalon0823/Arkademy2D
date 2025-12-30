using UnityEngine;
using UnityEngine.Events;

namespace Arkademy2D.Game.Behaviours.Actor
{
    public class Health : MonoBehaviour
    {
        public int faction;
        public int max;
        public int current;

        public UnityEvent onDamage;
        public void TakeDamage(int amount)
        {
            if (current <= 0) return;
            current -=  amount;
            current = Mathf.Clamp(current, 0, max);
            onDamage.Invoke();
        }
    }
}