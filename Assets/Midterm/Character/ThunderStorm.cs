using System.Collections;
using System.Linq;
using Midterm.Field;
using UnityEngine;

namespace Midterm.Character
{
    public class ThunderStorm : Spell
    {
        public GameObject lightningFXPrefab;

        public float interval;
        public float lastUse;

        public AudioSource audioSource;
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
                   
                    var candidates = WaveManager.Instance.spawnedEnemies.Where(x =>
                        x.life > 0 && Player.Player.Local.OnScreen(x.body.position)).ToList();
                    if (candidates.Count == 0) return;
                    lastUse = Time.timeSinceLevelLoad;
                    var group = Random.Range(-int.MaxValue, int.MaxValue);
                    var candidate = candidates[Random.Range(0, candidates.Count)];
                    var hits = Physics2D.OverlapCircleAll(candidate.body.position, 2.5f, LayerMask.GetMask("Enemy"));
                    foreach (var hit in hits)
                    {
                        var chara = hit.GetComponent<Character>();
                        if (!chara) continue;
                        if (chara.life > 0)
                        {
                            StartCoroutine(DoDamage(chara, 60,group));
                        }
                    }

                    var fx = Instantiate(lightningFXPrefab, candidate.body.position, Quaternion.identity);
                   audioSource.pitch = Random.Range(0.8f, 1.2f);
                    audioSource.PlayOneShot(audioSource.clip);
                }
            }
        }

        public IEnumerator DoDamage(Character chara, int damage,int group)
        {
            for (var i = 0; i < 4; i++)
            {
                chara.TakeDamage(damage,group);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}