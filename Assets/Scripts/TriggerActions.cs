using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy
{
    public class TriggerActions : MonoBehaviour
    {
        public bool triggered;
        public int damageAmount;

        public void DealDamage(Collider2D other)
        {
            var chara = other.GetComponent<CharacterBehaviour>();
            if (chara.gameObject.layer != gameObject.layer)
            {
                chara.TakeDamage(damageAmount);
            }
        }
    }
}