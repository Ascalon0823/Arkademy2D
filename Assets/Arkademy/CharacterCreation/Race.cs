using Arkademy.Common;
using UnityEngine;

namespace Arkademy.CharacterCreation
{
    [CreateAssetMenu(fileName = "New Race", menuName = "Data/Race", order = 0)]
    public class Race : ScriptableObject
    {
        public Resource energy = Resource.energy;
        public Attribute speed = Attribute.speed;
        public bool playable;
    }
}