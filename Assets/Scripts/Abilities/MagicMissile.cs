using System.Collections;
using UnityEngine;

namespace Arkademy.Abilities
{
    public class MagicMissile : Ability
    {
        public GuidedProjectile missilePrefab;
        public float spawnInterval;
        public float projectileSpeed;
        public int baseDamage;
        public float projectileLife;

        protected override void Use()
        {
            base.Use();
            useCount++;
            StartCoroutine(SpawnProjectiles());
        }

        protected virtual IEnumerator SpawnProjectiles()
        {
            var projectileCount = 2 + level / 2;
            var targets = user.GetNearestEnemies(projectileCount);

            for (var i = 0; i < projectileCount; i++)
            {
                var target = targets.Length > 0 ? targets[i % targets.Length] : null;
                var projectile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
                projectile.transform.up = target ? (target.transform.position - transform.position)
                    : user.wantToMove.sqrMagnitude > 0.001f ? user.wantToMove : Vector2.up;
                projectile.moveSpeed = projectileSpeed;
                projectile.damage = new DamageEvent()
                {
                    dealerInstance = instanceId,
                    batch = useCount,
                    amount = level * baseDamage
                };
                projectile.remainingLife = projectileLife;
                projectile.gameObject.SetLayerRecursive(gameObject.layer);
                projectile.target = target;
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}