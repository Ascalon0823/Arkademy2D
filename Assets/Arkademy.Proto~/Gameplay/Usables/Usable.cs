using System;
using UnityEngine;

namespace Arkademy.Gameplay.Usables
{
    public abstract class Usable : MonoBehaviour
    {
        public float useTime;
        [SerializeField] private float remainingUseTime;
        
        public virtual bool CanUse()
        {
            return remainingUseTime <= 0f;
        }
        protected virtual void Update()
        {
            if (remainingUseTime <= 0f) return;
            remainingUseTime -= Time.deltaTime;
        }

        public virtual void Use(Vector3 targetPosition)
        {
            remainingUseTime = useTime;
        }
    }
}