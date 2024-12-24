using System;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Ability : MonoBehaviour
    {
        public float cooldown;
        public float remainnigCooldown;
        public Character user;
        public virtual bool CanUse()
        {
            return remainnigCooldown <= 0 && !user.moving;
        }
        protected virtual void Update()
        {
            if(remainnigCooldown>0)
                remainnigCooldown -= Time.deltaTime;
        }

        public virtual void Use()
        {
            remainnigCooldown = cooldown;
        }
    }
}