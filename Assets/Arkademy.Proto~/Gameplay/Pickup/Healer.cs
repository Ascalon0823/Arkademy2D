using System;
using UnityEngine;
using Attribute = Arkademy.Data.Attribute;

namespace Arkademy.Gameplay.Pickup
{
    public class Healer : PickupBase
    {
        public int healAmount;
        
        protected override bool CanBePickupBy(Character character)
        {
            return character.Attributes[Attribute.Type.Life] != null && !character.isDead;
        }

        protected override bool Payload()
        {
            if (pickupCharacter && !pickupCharacter.isDead)
            {
                pickupCharacter.Heal(healAmount);
                return true;
            }

            return false;
        }
    }
}