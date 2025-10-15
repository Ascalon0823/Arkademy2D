using Midterm.Character;
using UnityEngine;

namespace Midterm.Field
{
    public class EnergyPickup : Pickup
    {
        public int energy;

        public override void PickupBy(Character.Character character)
        {
            base.PickupBy(character);
            character.GainEnergy(energy);
        }
    }
}