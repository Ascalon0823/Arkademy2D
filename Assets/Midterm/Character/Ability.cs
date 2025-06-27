using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Midterm.Character
{
    public class Ability : MonoBehaviour
    {
        [Serializable]
        public class Upgrade
        {
            public string name;
            public int currLevel;
            public int maxLevel;

            public Upgrade(string newName)
            {
                name = newName;
                currLevel = 0;
                maxLevel = 99;
            }

            public static Upgrade Power => new Upgrade("Power");
            public static Upgrade Range => new Upgrade("Range");
            public static Upgrade Speed => new Upgrade("Speed");
            public static Upgrade Size => new Upgrade("Size");
        }

        public string internalName;
        public Character user;
        public float useTime;
        public Sprite icon;
        public int currLevel;

        public int maxLevel;
        public float cooldown;
        [SerializeField] protected float remainingUseTime;
        [SerializeField] protected float lastUseTime;
        public float remainingCooldown;
        [SerializeField] protected float lastCooldown;

        public AudioSource AudioSource;
        public AudioClip[] useSounds;
        public AudioClip[] levelUpSounds;
        public AudioClip equipSound;
        

        public virtual List<Upgrade> GetAvailableUpgrades()
        {
            return new List<Upgrade>();
        }

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
            lastUseTime = remainingUseTime;
            remainingCooldown = GetCooldown() + remainingUseTime;
            lastCooldown = remainingCooldown;
            if (AudioSource && useSounds != null && useSounds.Length > 0)
            {
                AudioSource.clip = useSounds[Random.Range(0, useSounds.Length)];
                AudioSource.Play();
            }
        }

        public float GetCooldownPercentage()
        {
            return remainingCooldown / lastCooldown;
        }
    }
}