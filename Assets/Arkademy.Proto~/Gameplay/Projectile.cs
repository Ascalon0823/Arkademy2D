using System;
using System.Collections.Generic;
using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        public float remainingLife;
        public float speed;
        public Vector2 dir;
        public Rigidbody2D body;
        public bool rotateToDir;
        public Action<Collider2D> OnHit;
        public List<Collider2D> ignores = new();

        private void Start()
        {
            if (rotateToDir)
                transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        }

        private void FixedUpdate()
        {
            remainingLife -= Time.fixedDeltaTime;
            if (remainingLife <= 0)
            {
                Destroy(gameObject);
                return;
            }
            body.MovePosition(body.position + dir * speed * Time.fixedDeltaTime);
            if (rotateToDir)
                body.MoveRotation(Quaternion.FromToRotation(Vector3.up, dir));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (ignores.Contains(other)) return;
            OnHit?.Invoke(other);
        }
        public void Setup(int damage, int faction, bool passThrough)
        {
            OnHit += other =>
            {
                if (other.GetCharacter(out var character))
                {
                    if (character.faction == faction) return;
                    character.TakeDamage(new DamageData(damage));
                    Destroy(gameObject);
                    return;
                }

                if (passThrough) return;
                Destroy(gameObject);
            };
        }
    }
}