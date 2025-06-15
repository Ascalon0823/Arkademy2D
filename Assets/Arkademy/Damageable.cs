using UnityEngine;
using UnityEngine.Events;

namespace Arkademy
{
    public class Damageable : MonoBehaviour
    {
        public UnityEvent<int, Vector2> onDamageTakenAt;

        public void TakeDamageAt(int damage, Vector2 position)
        {
            onDamageTakenAt?.Invoke(damage, position);
        }
    }
}