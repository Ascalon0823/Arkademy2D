using System;
using UnityEngine;

namespace Midterm.Character
{
    public class Pickuper : MonoBehaviour
    {
        public float range;
        public Character character;
        private void FixedUpdate()
        {
            if(!character || character.life<=0)return;
            if (character.currSpell && character.currSpell.casting) return;
            var pickups = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Pickup"));
            foreach (var c in pickups)
            {
                var pickup = c.GetComponent<Pickup>();
                if(!pickup)return;
                pickup.PickupBy(character);
            }
        }
    }
}