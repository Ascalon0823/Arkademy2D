using System;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class DamageBox : MonoBehaviour
    {
        public int damage;
        public int faction;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetCharacter(out var chara) && chara.faction != faction)
            {
                chara.TakeDamage(damage);
            }
        }
    }
}