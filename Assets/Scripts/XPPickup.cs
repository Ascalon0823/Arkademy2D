using System;
using UnityEngine;

namespace Arkademy
{
    public class XPPickup : Pickup
    {
        public int xp;
        public static Transform XpRoot;

        private void Start()
        {
            if (!XpRoot) XpRoot = new GameObject("XpRoot").transform;
            if (transform.parent != XpRoot)
            {
                transform.SetParent(XpRoot, true);
            }
        }

        protected override void GrantPickupTo(CharacterBehaviour chara)
        {
            base.GrantPickupTo(chara);
            chara.GainXP(xp);
            Destroy(gameObject);
        }
    }
}