using UnityEngine;

namespace Arkademy
{
    public class EnemyBehaviour : CharacterBehaviour
    {
        public float destroyTimer;

        protected override void Update()
        {
            if (isDead)
            {
                rb.simulated = false;
                collider2d.enabled=false;
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
    }
}