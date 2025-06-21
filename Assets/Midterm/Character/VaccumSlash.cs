using System;
using UnityEngine;

namespace Midterm.Character
{
    public class VaccumSlash : Spell
    {
        public Projectile slashPrefab;
        public float interval;
        public float lastUse;

        private void Start()
        {
            lastUse = 0;
        }

        public override void Use(Vector2 pos)
        {
            base.Use(pos);
            if (casting)
            {
                if (lastUse == 0 || (Time.timeSinceLevelLoad - lastUse >= interval / user.attackSpeed))
                {
                    lastUse = Time.timeSinceLevelLoad;
                    var dir = (pos - (Vector2)transform.position).normalized;
                    var slash = Instantiate(slashPrefab, transform.position, Quaternion.identity);
                    slash.transform.up = dir;
                    slash.damage = Mathf.FloorToInt(slash.damage * user.power);
                    slash.ignores.Add(user.collider);
                }
            }
        }
    }
}