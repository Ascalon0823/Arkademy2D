using System;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class AbilityPayload : MonoBehaviour
    {
        public AbilityBase ability;
        public float duration;
        public virtual void Init(AbilityEventData data, AbilityBase parent, float dura)
        {
            ability = parent;
            duration = dura;
        }

        public virtual void UpdatePayload(AbilityEventData data)
        {
            
        }

        public virtual void Cancel()
        {
            
        }

        protected virtual void Update()
        {
            duration -= Time.deltaTime;
            if (duration < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}