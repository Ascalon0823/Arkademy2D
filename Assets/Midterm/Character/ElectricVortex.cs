using System.Collections.Generic;
using UnityEngine;

namespace Midterm.Character
{
    public class ElectricVortex : Spell
    {
        public GameObject vortexPrefab;
        
        [SerializeField] private List<GameObject> spawnedVortex = new List<GameObject>();

        public float lastUse;
        public float interval;

        public override void BeginUse(Vector2 pos)
        {
            base.BeginUse(pos);
            for (var i = 0; i < 3; i++)
            {
                var quat = Quaternion.Euler(0, 0, (i / 3f) * 360f);
                var spawn = Instantiate(vortexPrefab, user.transform.position + quat * Vector3.up * 4f, Quaternion.identity);
                spawn.transform.localScale = Vector3.one * 2f;
                spawnedVortex.Add(spawn);
            }
        }

        public override void Use(Vector2 pos)
        {
            base.Use(pos);
            if (casting)
            {
                if (lastUse == 0 || (Time.timeSinceLevelLoad - lastUse >= interval / user.attackSpeed))
                {
                    lastUse = Time.timeSinceLevelLoad;
                    foreach (var spawned in spawnedVortex)
                    {
                        var hits = Physics2D.OverlapCircleAll(spawned.transform.position, 4f);
                        foreach (var hit in hits)
                        {
                            var character = hit.transform.GetComponent<Character>();
                            if (!character || character == user)
                            {
                                continue;
                            }
                            character.TakeDamage(50);
                            character.knockBackDir =
                                (spawned.transform.position - character.transform.position).normalized * 2f;
                        }
                    }
                }
            }
        }

        public override void EndUse(Vector2 pos)
        {
            base.EndUse(pos);
            foreach (var spawned in spawnedVortex)
            {
                if(spawned)Destroy(spawned);
            }
        }
    }
}