using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy
{
    public class Ability : MonoBehaviour
    {
        public CharacterBehaviour user;
        public string abilityName;
        public Sprite uiIcon;
        public int level;
        public float cooldown;
        public float remainingCooldown;
        public int instanceId;
        public int useCount;

        private void Start()
        {
            instanceId = GetInstanceID();
        }

        protected virtual void Update()
        {
            if (Player.Paused) return;
            if (cooldown.Equals(0f) && useCount > 0) return; //Use only once
            remainingCooldown -= Time.deltaTime;
            if (remainingCooldown > 0f) return;
            Use();
        }

        public virtual void OnLevelUp()
        {
            
        }
        protected virtual void Use()
        {
            remainingCooldown = cooldown;
            useCount++;
        }
    }
}