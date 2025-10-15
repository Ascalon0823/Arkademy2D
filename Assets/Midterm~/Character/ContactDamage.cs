using System;
using UnityEngine;

namespace Midterm.Character
{
    public class ContactDamage : MonoBehaviour
    {
        public int damage;
        public float interval;
        [SerializeField] private Character contactCharacter;
        [SerializeField] private float lastDamageTime;
        public Character dealer;

        private void OnCollisionEnter2D(Collision2D other)
        {
            var character = other.gameObject.GetComponent<Character>();
            if (IsValidCharacter(character))
            {
                contactCharacter = character;
            }
        }

        protected virtual bool IsValidCharacter(Character character)
        {
            return character && character == Player.Player.Local.currCharacter && character.life > 0 && !character.collider.isTrigger;
        }

        private void FixedUpdate()
        {
            if (!contactCharacter) return;
            if (Time.timeSinceLevelLoad - lastDamageTime > interval)
            {
                DealDamage(contactCharacter);
            }
        }

        protected virtual void DealDamage(Character character)
        {
            character.TakeDamage(Mathf.FloorToInt(damage * dealer.power));
            lastDamageTime = Time.timeSinceLevelLoad;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var character = other.gameObject.GetComponent<Character>();
            if (character == contactCharacter)
            {
                contactCharacter = null;
            }
        }
    }
}