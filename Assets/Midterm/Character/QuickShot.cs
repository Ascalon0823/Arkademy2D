using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Midterm.Field;
using UnityEngine;

namespace Midterm.Character
{
    public class QuickShot : Ability
    {
        public Projectile projectilePrefab;
        public Upgrade power = Upgrade.Power;
        public Upgrade size = Upgrade.Size;
        public Upgrade speed = Upgrade.Speed;
        public Upgrade amount = Upgrade.Amount;
        public override List<Upgrade> GetAvailableUpgrades()
        {
            return new List<Upgrade>
            {
                power, size, speed, amount
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
        public override void Use()
        {
            base.Use();
            StartCoroutine(SpawnProjectileCoroutine());
        }

        public IEnumerator SpawnProjectileCoroutine()
        {
            var count = 2 + amount.currLevel;
            var interval = GetUseTime()/count;
            var nearest = WaveManager.Instance.spawnedEnemies.Where(x => x.life > 0)
                .OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
                .FirstOrDefault();
            var targetPos = nearest ? nearest.transform.position : transform.position;
            var pos = transform.position;
            var dir = (targetPos - pos).normalized;
            var group = Mathf.FloorToInt(Time.timeSinceLevelLoad * 1000);
            for (var i = 0; i < 2 + amount.currLevel; i++)
            {
                var projectile = Instantiate(projectilePrefab, pos, Quaternion.identity);
                
                projectile.transform.up = dir;
                projectile.damage = Mathf.FloorToInt(projectile.damage * (1+power.currLevel/2f));
                projectile.ignores.Add(user.collider);
                projectile.group = group;
                projectile.transform.localScale *= 1+(size.currLevel/2f);
                if (AudioSource && useSounds != null && useSounds.Length > 0)
                {
                    AudioSource.PlayOneShot(useSounds[Random.Range(0, useSounds.Length)]);
                }
                yield return new WaitForSeconds(interval);
            }
        }
    }
}