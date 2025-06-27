using System;
using Midterm.Field;
using UnityEngine;

namespace Midterm.Character
{
    public class FireAura : Ability
    {
        public int damage;
        public Transform circle;
        public float radius;

        public override void Use()
        {
            base.Use();
            var enemies = Physics2D.OverlapCircleAll(circle.position, radius+currLevel/2f);
            foreach (var c in enemies)
            {
                var e = c.GetComponent<Enemy>();
                if (e)
                {
                    e.character.TakeDamage(Mathf.FloorToInt(damage * user.power * (1+currLevel/5f)));
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            circle.localScale = Vector3.one * (radius + currLevel/2f);
        }
    }
}