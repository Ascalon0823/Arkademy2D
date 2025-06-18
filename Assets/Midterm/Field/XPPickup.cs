using Midterm.Character;

namespace Midterm.Field
{
    public class XPPickup : Character.Pickup
    {
        public int xp;

        public override void PickupBy(Character.Character character)
        {
            base.PickupBy(character);
            character.GetComponent<Level>().AddXp(xp);
            Destroy(gameObject);
        }
    }
}