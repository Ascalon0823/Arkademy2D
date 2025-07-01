using System;
using UnityEngine;

namespace Midterm.Character
{
    public class DamageTrigger : MonoBehaviour
    {
        public int damage;

        public float knockbackPower;
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.IsChildOf(transform.root))
            {
                return;
            }
            var character = other.GetComponent<Character>();
            if (!character) return;
            character.TakeDamage(damage);
            character.knockBackDir += (Vector2)(character.transform.position - transform.position).normalized * knockbackPower;
        }
    }
}