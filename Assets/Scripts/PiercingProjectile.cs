using UnityEngine;

namespace Arkademy
{
    public class PiercingProjectile : Projectile
    {
        public int pierceAmount;
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == gameObject.layer) return;
            var chara = other.GetComponent<CharacterBehaviour>();
            if (chara)
            {
                OnHitCharacter?.Invoke(chara);
                pierceAmount--;
            }

            if (pierceAmount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}