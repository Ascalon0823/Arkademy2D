using System.Collections;
using UnityEngine;

namespace Arkademy.Abilities
{
    public class WindCutter : Ability
    {
        public PiercingProjectile projectilePrefab;
        public int angleSpread;
        public int baseDamage;
        public float projectileLife;
        public int pierceAmount;
        public float projectileSpeed;
        public float interval;

        protected override void Use()
        {
            base.Use();
            useCount++;
            StartCoroutine(SpawnProjectiles());
        }

        protected virtual IEnumerator SpawnProjectiles()
        {
            var count = 1 + level;
            var fromAngle = (count - 1) * angleSpread / 2f;
            for (var i = 0; i < count * 2; i++)
            {
                var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.transform.eulerAngles =
                    new Vector3(0, 0, fromAngle - angleSpread * (i/2) - (i % 2 == 1 ? 180f : 0f));
                projectile.moveSpeed = projectileSpeed;
                projectile.damage = new DamageEvent
                {
                    dealerInstance = GetInstanceID(),
                    amount = baseDamage * level,
                    batch = useCount
                };
                projectile.remainingLife = projectileLife;
                projectile.gameObject.SetLayerRecursive(gameObject.layer);
                projectile.pierceAmount = pierceAmount + level;
                projectile.transform.localScale = Vector3.one * 0.5f * (1+level/3);
                if(i%2==1)
                    yield return new WaitForSeconds(interval);
            }

            yield return null;
        }
    }
}