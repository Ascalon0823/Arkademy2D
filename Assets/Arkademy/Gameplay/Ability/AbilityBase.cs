using System;
using UnityEngine;

namespace Arkademy.Gameplay.Ability
{
    public class AbilityBase : MonoBehaviour
    {
        public float cooldown;
        public float remainingCooldown;
        public Character user;
        public virtual bool CanUse(Character target)
        {
            return remainingCooldown <= 0 && !user.moving;
        }
        protected virtual void Update()
        {
            if(remainingCooldown>0)
                remainingCooldown -= Time.deltaTime;
        }

        public virtual void Use(Character target)
        {
            remainingCooldown = cooldown;
        }
    }
}