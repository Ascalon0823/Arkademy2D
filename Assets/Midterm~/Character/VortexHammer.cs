using System;
using System.Collections.Generic;
using System.Linq;
using Midterm.Field;
using UnityEngine;

namespace Midterm.Character
{
    public class VortexHammer : Spell
    {
        public DamageTrigger hammerPrefab;

        public float interval;
        public float lastUse;

        public List<DamageTrigger> damageTriggers = new List<DamageTrigger>();
        private Dictionary<DamageTrigger, float> damageTriggerTimes = new Dictionary<DamageTrigger, float>();

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
                    var trigger = Instantiate(hammerPrefab, user.transform.position, Quaternion.identity);
                    trigger.GetComponent<Collider2D>().excludeLayers|=LayerMask.GetMask("Player");
                    lastUse = Time.timeSinceLevelLoad;
                    damageTriggers.Add(trigger);
                    trigger.damage = 250;
                    trigger.knockbackPower = 1;
                    damageTriggerTimes[trigger] = 0;
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (var damageTrigger in damageTriggers)
            {
                if (!damageTrigger) continue;
                var t = damageTriggerTimes[damageTrigger];
              
                damageTrigger.transform.position = user.transform.position +
                                                   Quaternion.Euler(0, 0, t * 360f) * Vector3.up *
                                                   Mathf.Clamp(t, 0f, 1f)*10f;
                damageTrigger.transform.localEulerAngles += new Vector3(0f, 0f, Time.fixedDeltaTime * 180f);
                damageTriggerTimes[damageTrigger] += Time.fixedDeltaTime;
            }
        }

        public override void EndUse(Vector2 pos)
        {
            base.EndUse(pos);
            foreach (var damageTrigger in damageTriggers)
            {
                Destroy(damageTrigger.gameObject);
            }
        }
    }
}