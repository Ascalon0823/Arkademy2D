using UnityEngine;

namespace Arkademy
{
    public class XPPickup : Pickup
    {
        public int xp;
        protected override void GrantPickupTo(CharacterBehaviour chara)
        {
            base.GrantPickupTo(chara);
            chara.GainXP(xp);
            Destroy(gameObject);
        }
    }
}