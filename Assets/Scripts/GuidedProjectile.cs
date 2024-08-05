using UnityEngine;

namespace Arkademy
{
    public class GuidedProjectile : Projectile
    {
        public CharacterBehaviour target;
        public bool unblockable;
        public bool rotateUp;

        protected override void UpdatePosition()
        {
            if (!target)
            {
                base.UpdatePosition();
                return;
            }

         
            var displacement = target.transform.position - transform.position;
            if (rotateUp)
            {
                transform.up = displacement.normalized;
            }
            transform.position +=
                Mathf.Min(moveSpeed * Time.deltaTime, displacement.magnitude) * displacement.normalized;
        }


        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!unblockable)
            {
                base.OnTriggerEnter2D(other);
                return;
            }
            if (other.gameObject.layer == gameObject.layer) return;
            var chara = other.GetComponent<CharacterBehaviour>();
            if (chara != target || !chara || !target) return;
            chara.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}