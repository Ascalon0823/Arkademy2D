using System.Collections;
using UnityEngine;

namespace Arkademy.Abilities
{
    public class MagicMissile : Ability
    {
        public Projectile missilePrefab;
        public float spawnInterval;
        public float projectileSpeed;
        public int baseDamage;
        public float projectileLife;

        protected override void Use()
        {
            base.Use();
            StartCoroutine(SpawnProjectiles());
        }

        protected virtual IEnumerator SpawnProjectiles()
        {
            var projectileCount = 1 + level / 2f;
            var target = user.GetNearestEnemy();
            for (var i = 0; i < projectileCount; i++)
            {
                var projectile = Instantiate(missilePrefab,transform.position,Quaternion.identity);
                projectile.transform.up = target ? (target.transform.position - transform.position)
                    : user.wantToMove.sqrMagnitude > 0.001f ? user.wantToMove : Vector2.up;
                projectile.moveSpeed = projectileSpeed;
                projectile.damage = level * baseDamage;
                projectile.remainingLife = projectileLife;
                projectile.gameObject.SetLayerRecursive(gameObject.layer);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}