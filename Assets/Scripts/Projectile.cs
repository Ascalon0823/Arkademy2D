using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy
{
    public class Projectile : MonoBehaviour
    {
        public float moveSpeed;
        public float remainingLife;
        public UnityEvent<CharacterBehaviour> OnHitCharacter;

        protected virtual void Update()
        {
            UpdateLife();
            UpdatePosition();
        }

        protected virtual void UpdateLife()
        {
            if (remainingLife <= 0f)
            {
                Destroy(gameObject);
            }

            remainingLife -= Time.deltaTime;
        }

        protected virtual void UpdatePosition()
        {
            transform.position += moveSpeed * Time.deltaTime * transform.up;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == gameObject.layer) return;
            var chara = other.GetComponent<CharacterBehaviour>();
            if (chara)
            {
                OnHitCharacter?.Invoke(chara);
            }
            Destroy(gameObject);
        }
    }
}