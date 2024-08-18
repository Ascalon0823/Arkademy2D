using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy
{
    public class TriggerActions : MonoBehaviour
    {
        public bool triggered;
        public int damageAmount;

        [SerializeField] private int triggerCount;
        public void DealDamage(Collider2D other)
        {
            var chara = other.GetComponent<CharacterBehaviour>();
            
            if (chara && chara.gameObject.layer != gameObject.layer)
            {
                triggerCount++;
                chara.TakeDamage(new DamageEvent
                {
                    dealerInstance = GetInstanceID(),
                    batch = triggerCount,
                    amount = damageAmount
                });
            }
        }
    }
}