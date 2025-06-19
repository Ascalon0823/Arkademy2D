using System;
using UnityEngine;

namespace Midterm.Character
{
    public class Ability : MonoBehaviour
    {
        public Character user;
        public float useTime;

        public float cooldown;
        [SerializeField] protected float remainingUseTime;
        [SerializeField] protected float remainingCooldown;

        public virtual bool CanUse()
        {
            return remainingUseTime <= 0 && remainingCooldown <= 0;
        }

        public virtual float GetUseTime()
        {
            return useTime / user.attackSpeed;
        }

        public virtual float GetCooldown()
        {
            return cooldown / user.attackSpeed;
        }

        protected virtual void Update()
        {
            UpdateUseTime();
            UpdateCooldown();
        }

        protected virtual void UpdateCooldown()
        {
            if (remainingCooldown <= 0) return;
            remainingCooldown -= Time.deltaTime;
        }

        protected virtual void UpdateUseTime()
        {
            if (remainingUseTime <= 0) return;
            remainingUseTime -= Time.deltaTime;
        }

        public virtual void Use()
        {
            remainingUseTime = GetUseTime();
            remainingCooldown = GetCooldown() + remainingUseTime;
        }
    }
}