using System;
using System.Linq;
using Midterm.Field;
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
                    var nearest = WaveManager.Instance.spawnedEnemies.
                        Where(x=>x.life>0)
                        .OrderBy(x => Vector3.Distance(transform.position, x.body.position))
                        .FirstOrDefault();
                    var dir = nearest
                        ? (nearest.body.position - (Vector2)transform.position)
                        : user.faceDir;
                    var slash = Instantiate(slashPrefab, transform.position, Quaternion.identity);
                    slash.transform.up = dir.normalized;
                    slash.damage = Mathf.FloorToInt(slash.damage * user.power);
                    slash.ignores.Add(user.collider);
                }
            }
        }
    }
}