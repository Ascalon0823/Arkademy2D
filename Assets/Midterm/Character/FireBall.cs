using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Midterm.Field;
using UnityEngine;

namespace Midterm.Character
{
    public class FireBall : Ability
    {
        public Projectile fireBallProjectile;
        public GameObject explosionPrefab;
        public Upgrade power = Upgrade.Power;
        public Upgrade size = Upgrade.Size;
        public Upgrade speed = Upgrade.Speed;
        public Upgrade amount = Upgrade.Amount;
        
        public AudioClip explodeSound;
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
            var count = 1 + amount.currLevel;
            var interval = GetUseTime()/count;
            var nearest = WaveManager.Instance.spawnedEnemies.Where(x => x.life > 0)
                .OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
                .FirstOrDefault();
            var targetPos = nearest ? nearest.transform.position : transform.position;
            var pos = transform.position;
            var dir = (targetPos - pos).normalized;
            var group = Mathf.FloorToInt(Time.timeSinceLevelLoad * 1000);
            for (var i = 0; i < count; i++)
            {
                var projectile = Instantiate(fireBallProjectile, pos, Quaternion.identity);
                projectile.transform.up = dir;
                projectile.damage = 0;
                projectile.ignores.Add(user.collider);
                projectile.group = group;
                projectile.transform.localScale *= 1+(size.currLevel/2f);
                projectile.onHit += (c) =>
                {
                    var chara = c.GetComponent<Character>();
                    if (!chara || chara.life <= 0|| chara == user) return;
                    var p = c.ClosestPoint(projectile.transform.position);
                    var exp = Instantiate(explosionPrefab, p, Quaternion.identity);
                    exp.transform.localScale = Vector3.one * 2f* (1+size.currLevel/2f);
                    AudioSource.PlayOneShot(explodeSound, Random.Range(0.8f, 1.2f));
                    var colliders = Physics2D.OverlapCircleAll(p, 2f*(1+size.currLevel/2f),LayerMask.GetMask("Enemy")) ;
                    foreach (var cc in colliders)
                    {
                        var otherchara = cc.GetComponent<Character>();
                        if (!otherchara || otherchara.life <= 0 || otherchara == user) return;
                        otherchara.TakeDamage(Mathf.FloorToInt(300 * (1+power.currLevel/2f)));
                        otherchara.knockBackDir = (otherchara.body.position - p).normalized * 3f;
                    }
                };
                if (AudioSource && useSounds != null && useSounds.Length > 0)
                {
                    AudioSource.PlayOneShot(useSounds[Random.Range(0, useSounds.Length)]);
                }
                yield return new WaitForSeconds(interval);
            }
        }
    }
}