using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Midterm.Field;
using UnityEngine;

namespace Midterm.Character
{
    public class CorpseExplosion : Ability
    {
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
            var interval = 0.5f;
            var corpses = WaveManager.Instance.corpse
                .Where(x => Vector3.Distance(x.transform.position, transform.position) < 10).ToList();
            for (var i = 0; i < count; i++)
            {
                var candidateCorpse = corpses[Random.Range(0, corpses.Count)];
                var p = candidateCorpse.transform.position;
                var exp = Instantiate(explosionPrefab, p, Quaternion.identity);
                exp.transform.localScale = Vector3.one * 2f* (1+size.currLevel/2f);
                AudioSource.PlayOneShot(explodeSound, Random.Range(0.8f, 1.2f));
                var colliders = Physics2D.OverlapCircleAll(p, 2f*(1+size.currLevel/2f),LayerMask.GetMask("Enemy")) ;
                foreach (var cc in colliders)
                {
                    var otherchara = cc.GetComponent<Character>();
                    if (!otherchara || otherchara.life <= 0 || otherchara == user) continue;
                    otherchara.TakeDamage(Mathf.FloorToInt(candidateCorpse.maxLife * (1+power.currLevel/2f)));
                    otherchara.knockBackDir = (otherchara.body.position - (Vector2)p).normalized * 1.5f;
                }

                WaveManager.Instance.corpse.Remove(candidateCorpse);
                corpses.Remove(candidateCorpse);
                Destroy(candidateCorpse);
                yield return new WaitForSeconds(interval);
            }
        }
    }
}