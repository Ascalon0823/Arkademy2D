using System;
using System.Collections.Generic;
using UnityEngine;

namespace Midterm.Character
{
    public class Projectile : MonoBehaviour
    {
        public int damage;
        public float speed;
        public List<Collider2D> ignores = new List<Collider2D>();
        public float life;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (ignores.Contains(other)) return;
            var chara = other.GetComponent<Character>();
            if(!chara) return;
            if (chara.life > 0)
            {
                chara.TakeDamage(damage);
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