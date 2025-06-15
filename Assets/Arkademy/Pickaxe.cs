using UnityEngine;

namespace Arkademy
{
    public class Pickaxe : Usable
    {
        public float range;
        public int damage;
        public override void Use(Vector2 position)
        {
            base.Use(position);
            var dir = position - (Vector2)transform.position;
            dir = dir.normalized;
            var hit = Physics2D.Raycast(transform.position, dir.normalized, range);
            if (!hit.collider) return;
            var damageable = hit.collider.GetComponent<Damageable>();
            if (!damageable) return;
            damageable.TakeDamageAt(damage, hit.point);
        }
    }
}