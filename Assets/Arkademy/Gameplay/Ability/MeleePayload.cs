using System;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class MeleePayload : MonoBehaviour
    {
        public Trigger trigger;

        public float remainingLife;
        [Range(0f, 1f)] public float triggerPointPercentage;
        public float triggerPoint;

        private void Awake()
        {
            trigger.trigger.enabled = false;
        }

        private void Update()
        {
            remainingLife -= Time.deltaTime;
            if (remainingLife < triggerPoint) trigger.trigger.enabled = true;
            if (remainingLife <= 0) Destroy(gameObject);
        }
    }
}