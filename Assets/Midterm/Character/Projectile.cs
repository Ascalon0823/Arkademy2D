using System;
using System.Collections.Generic;
using UnityEngine;

namespace Midterm.Character
{
    public class Projectile : MonoBehaviour
    {
        public int damage;
        public int group=-1;
        public float speed;
        public List<Collider2D> ignores = new List<Collider2D>();
        public float life;
        public int pierce;
        public Action<Collider2D> onHit;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (ignores.Contains(other)) return;
            onHit?.Invoke(other);
            var chara = other.GetComponent<Character>();
            if(!chara) return;
            if (chara.life > 0)
            {
                chara.TakeDamage(damage, group);
            }

            if (pierce >= 0)
            {
                pierce--;
                if (pierce < 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void Update()
        {
            life-=Time.deltaTime;
            if (life <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }
}