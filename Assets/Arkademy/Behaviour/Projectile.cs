using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy.Behaviour
{
    public class Projectile : MonoBehaviour
    {
        public SpriteRenderer graphic;
        public Motor motor;
        public float remainingLife;
        public Trigger trigger;
        public int triggerCount;

        public UnityEvent<GameObject> onHit;
        public UnityEvent onLifeEnd;

        private void FixedUpdate()
        {
            remainingLife -= Time.fixedDeltaTime;
            if (remainingLife <= 0)
            {
                onLifeEnd?.Invoke();
                Destroy(gameObject);
            }
        }

        public void Hit(GameObject go)
        {
            if (triggerCount == 0) return;
            onHit?.Invoke(go);
        }

        public void Hit(Collider2D other)
        {
            if (triggerCount == 0) return;
            onHit?.Invoke(other.gameObject);
        }

        public void Hit(Transform other)
        {
            if (triggerCount == 0) return;
            onHit?.Invoke(other.gameObject);
        }
    }
}