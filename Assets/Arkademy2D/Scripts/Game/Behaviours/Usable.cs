using System;
using System.Collections.Generic;
using System.Linq;
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
        public float targetingRange;
        public int targetingFactionDiff;

        public bool CanUse(bool requireTarget = false)
        {
            return remainingUseTime <= 0 && remainingReuseTime <= 0 && user.energy >= energyRequirement
                && (!requireTarget || HasTarget());
        }

        public bool HasTarget()
        {
            var targets = GetTargets();
            return targets.Any();
        }

        public List<Character> GetTargets()
        {
            return Physics2D.OverlapCircleAll(transform.position, targetingRange)
                .Select(x => x.GetComponent<Character>())
                .Where(x => x && Mathf.Abs(x.faction- user.faction)==targetingFactionDiff && x.hp > 0)
                .OrderByDescending(x => Vector2.Distance(x.transform.position, x.transform.position))
                .ToList();
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, targetingRange);
        }
    }
}