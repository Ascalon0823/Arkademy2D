using System;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours
{
    public class Usable : MonoBehaviour
    {
        public float remainingUseTime;
        public float useTime;
        public float reuseTime;
        public float remainingReuseTime;
        public Usage activeUsage;
        public Usage usagePrefab;
        public Character user;
        public int energyRequirement;

        public bool CanUse()
        {
            return remainingUseTime <= 0 && remainingReuseTime <= 0 && user.energy >= energyRequirement;
        }

        public void Use()
        {
            if (!CanUse()) return;
            
            activeUsage = Instantiate(usagePrefab, transform.position, Quaternion.identity);
            activeUsage.Init(this);
            remainingReuseTime = reuseTime + useTime;
            remainingUseTime = useTime;
            user.ConsumeEnergy(energyRequirement);
        }

        private void Update()
        {
            remainingUseTime -= Time.deltaTime;
            remainingUseTime = Mathf.Max(0, remainingUseTime);
            remainingReuseTime -= Time.deltaTime;
            remainingReuseTime = Mathf.Max(0, remainingReuseTime);
        }
    }
}