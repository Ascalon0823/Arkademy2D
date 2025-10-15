using UnityEngine;

namespace Midterm.Character
{
    public class Pickup : MonoBehaviour
    {
        public virtual void PickupBy(Character character)
        {
            Destroy(gameObject);
        }
    }
}