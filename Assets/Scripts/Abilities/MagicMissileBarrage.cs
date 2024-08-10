using System.Collections;
using System.Linq;
using UnityEngine;

namespace Arkademy.Abilities
{
    public class MagicMissileBarrage : Spell
    {
        public float interval;
        public float projectileInterval;
        public int batchCount;
        public float sourceCost;
        public float range;
        public float radius;
        public float projectileSpeed;
        [SerializeField] private GuidedProjectile missilePrefab;
        [SerializeField] private float lastUseTime;

        public int useCount;
        public int damage;
        public float projectileLife;
        [SerializeField] private Transform indicator;

        public override bool OnUse(SpellUsage usage)
        {
            if (!user) return false;
            if (user.source < sourceCost)
            {
                Destroy(gameObject);
                return false;
            }
            if (usage.Phase == Phase.Begin) useCount = 0;
            var targetLocation = user.transform.position + (Vector3)usage.Direction * range;
            indicator.transform.position = targetLocation;
            indicator.transform.localScale = radius * Vector3.one;
            if (Time.timeSinceLevelLoad - lastUseTime > interval)
            {
                useCount++;
                user.source -= sourceCost;
                lastUseTime = Time.timeSinceLevelLoad;
                var targets = StageBehaviour.Current.spawnedCharacters
                    .Where(x => x.gameObject.layer != gameObject.layer)
                    .OrderBy(x => Vector2.Distance(
                        x.transform.position, targetLocation)).ToArray();
                var targetsInRange = targets.Where(x =>
                    Vector2.Distance(
                        x.transform.position, targetLocation) < radius).ToArray();
                var finalTargets = targetsInRange.Length != 0 ? targetsInRange : targets;
                if (finalTargets.Length == 0) return true;
                StartCoroutine(SpawnProjectile(finalTargets));
            }

            if (usage.Phase == Phase.End)
            {
                Destroy(gameObject);
            }

            return true;
        }

        IEnumerator SpawnProjectile(CharacterBehaviour[] finalTargets)
        {
          
           
            for (var i = 0; i < batchCount; i++)
            {
                var target = finalTargets[i % finalTargets.Length];
                var missile = Instantiate(missilePrefab, user.transform.position, Quaternion.identity);
                missile.transform.up = target ? (target.transform.position - transform.position)
                    : user.wantToMove.sqrMagnitude > 0.001f ? user.wantToMove : Vector2.up;
                missile.moveSpeed = projectileSpeed;
                missile.damage = new DamageEvent()
                {
                    dealerInstance = instanceId,
                    batch = useCount,
                    amount = damage
                };
                missile.remainingLife = projectileLife;
                missile.gameObject.SetLayerRecursive(gameObject.layer);
                missile.target = target;
                yield return new WaitForSeconds(projectileInterval);
            }
        }
    }
}