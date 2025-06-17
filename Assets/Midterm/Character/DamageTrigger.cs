using System;
using UnityEngine;

namespace Midterm.Character
{
    public class DamageTrigger : MonoBehaviour
    {
        public int damage;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.IsChildOf(transform.root))
            {
                return;
            }
            var character = other.GetComponent<Character>();
            if (!character) return;
            character.TakeDamage(damage);
        }
    }
}