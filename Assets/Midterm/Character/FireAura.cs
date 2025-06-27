using System;
using System.Collections.Generic;
using Midterm.Field;
using UnityEngine;

namespace Midterm.Character
{
    public class FireAura : Ability
    {
        public int damage;
        public Transform circle;
        public float radius;

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
            return base.GetUseTime() * (1f-speed.currLevel*0.1f);
        }

        public override void Use()
        {
            base.Use();
            var enemies = Physics2D.OverlapCircleAll(circle.position, radius+size.currLevel/2f);
            foreach (var c in enemies)
            {
                var e = c.GetComponent<Enemy>();
                if (e)
                {
                    e.character.TakeDamage(Mathf.FloorToInt(damage * user.power * (1+power.currLevel/5f)));
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            circle.localScale = Vector3.one * (radius + size.currLevel/2f);
        }
    }
}