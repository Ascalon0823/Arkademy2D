using System.Collections.Generic;
using System.Linq;
using Midterm.Field;
using UnityEngine;

namespace Midterm.Character
{
    public class ChainLightning : Spell
    {
        public DamageTrigger lightningSegment;
        
        public float interval;
        public float lastUse;

        public Character lastTarget;
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
                    lastUse = Time.timeSinceLevelLoad;
                    var from = lastTarget ? lastTarget : user;
                    var candidates = WaveManager.Instance.spawnedEnemies.Where(x =>
                        x.life > 0 && Player.Player.Local.OnScreen(x.body.position))
                        .OrderBy(x=>Mathf.Abs(Vector2.Distance(x.body.position, from.body.position) - 6f)).ToList();
                    if (candidates.Count == 0)
                    {
                        EndUse(pos);
                        return;
                    }
                    var candidate = candidates[0];
                    lastTarget = candidate;
                    var segment = Instantiate(lightningSegment, candidate.body.position, Quaternion.identity);
                    segment.GetComponent<BoxCollider2D>().excludeLayers|=LayerMask.GetMask("Player");
                    segment.transform.up = (from.transform.position - candidate.transform.position).normalized;
                    segment.transform.localScale = new Vector3(1f, (from.transform.position - candidate.transform.position).magnitude/6f, 1f);
                    segment.damage = 150;
                    audioSource.pitch = Random.Range(0.8f, 1.2f);
                    audioSource.PlayOneShot(audioSource.clip);
                }
            }
        }
    }
}