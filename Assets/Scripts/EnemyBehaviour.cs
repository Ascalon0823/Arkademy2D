using System;
using Arkademy.UI.Game;
using UnityEngine;

namespace Arkademy
{
    public class EnemyBehaviour : CharacterBehaviour
    {
        public float destroyTimer;
        public int xpDrop;
        public XPPickup xpPickupPrefab;
        public bool indestructible;

        protected override void Start()
        {
            base.Start();
            onDeath.AddListener(chara => { Drop(); });
        }

        protected virtual void Drop()
        {
            var pickup = Instantiate(xpPickupPrefab, transform.position, Quaternion.identity);
            pickup.xp = xpDrop;
        }

        protected override void Update()
        {
            if (isDead)
            {
                rb.simulated = false;
                collider2d.enabled = false;
                destroyTimer -= Time.deltaTime;
                if (destroyTimer < 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                wantToMove = Vector2.zero;
                if (PlayerBehaviour.PlayerChar && !PlayerBehaviour.PlayerChar.isDead)
                {
                    wantToMove = (PlayerBehaviour.PlayerChar.transform.position - transform.position).normalized;
                }
            }

            base.Update();
        }

        public override void TakeDamage(DamageEvent de)
        {
            if (indestructible)
            {
                if (showDamageNumber)
                    DamageTextCanvas.AddTextTo(transform, de);
                isHit = true;
                return;
            }

            base.TakeDamage(de);
        }
    }
}