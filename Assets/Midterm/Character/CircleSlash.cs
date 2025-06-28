using System.Collections.Generic;
using UnityEngine;

namespace Midterm.Character
{
    public class CircleSlash : Ability
    {
        [SerializeField] protected GameObject slash;
        [SerializeField] protected Animator slashAnimator;
        [SerializeField] protected DamageTrigger damageTrigger;

        public Upgrade power = Upgrade.Power;
        public Upgrade size = Upgrade.Size;
        public Upgrade speed = Upgrade.Speed;

        public override List<Upgrade> GetAvailableUpgrades()
        {
            return new List<Upgrade>
            {
                power, size, speed
            };
        }

        public override float GetUseTime()
        {
            return base.GetUseTime() * (1-speed.currLevel * 0.1f);
        }

        public override float GetCooldown()
        {
            return base.GetCooldown() * (1-speed.currLevel * 0.1f);
        }

        protected override void Update()
        {
            damageTrigger.damage = Mathf.FloorToInt(100 * user.power * (1 + power.currLevel/2f));
            base.Update();
            if (remainingUseTime <= 0)
            {
                slash.gameObject.SetActive(false);
            }
        }

        public override void Use()
        {
            base.Use();
            slash.SetActive(true);
            slash.transform.localPosition = Vector3.zero;
            slash.transform.localScale = Vector3.one  * (1 + size.currLevel/2f);
            slashAnimator.SetFloat("speed", 1f / GetUseTime());
        }
    }
}