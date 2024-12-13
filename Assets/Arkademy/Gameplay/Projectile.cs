using System;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        public float remainingLife;
        public bool passThroughObstacles;
        public float speed;
        public Vector2 dir;
        public Rigidbody2D body;
        public int faction;
        public int damage;
        public bool rotateToDir;
        private void FixedUpdate()
        {
            remainingLife -= Time.fixedDeltaTime;
            if (remainingLife <= 0)
            {
                Destroy(gameObject);
                return;
            }
            body.MovePosition(body.position + dir * speed * Time.fixedDeltaTime);
            if(rotateToDir)
                body.MoveRotation(Quaternion.FromToRotation(Vector3.up, dir));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetCharacter(out var character) && character.faction != faction)
            {
                character.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (passThroughObstacles) return;
            Destroy(gameObject);
        }
    }
}