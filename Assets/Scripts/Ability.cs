using System;
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

        protected virtual void Update()
        {
            if (PlayerBehaviour.Player.paused) return;
            remainingCooldown -= Time.deltaTime;
            if (remainingCooldown > 0f) return;
            Use();
        }

        protected virtual void Use()
        {
            remainingCooldown = cooldown;
        }
    }
}