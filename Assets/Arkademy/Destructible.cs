using UnityEngine;
using UnityEngine.Events;

namespace Arkademy
{
    public class Destructible : MonoBehaviour
    {
        public int life;
        public int maxLife;
        public UnityEvent onDestruction;

        public virtual void TakeDamage(int damage, Vector2 position)
        {
            life -= damage;
            if (life <= 0)
            {
                onDestruction?.Invoke();
            }
        }
    }
}