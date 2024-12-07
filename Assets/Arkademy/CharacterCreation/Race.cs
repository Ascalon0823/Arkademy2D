using Arkademy.Common;
using UnityEngine;

namespace Arkademy.CharacterCreation
{
    [CreateAssetMenu(fileName = "New Race", menuName = "Data/Race", order = 0)]
    public class Race : ScriptableObject
    {
        public Resource energy = Resource.energy;
        public Resource source = Resource.source;
        public Resource health = Resource.health;
        public Attribute speed = Attribute.speed;
        public Attribute castSpeed = Attribute.castSpeed;
        public Attribute attack = Attribute.attack;
        public Attribute defence = Attribute.defence;
        public bool playable;

        public Character CreateCharacter()
        {
            return new Character
            {
                raceName = name,
                energy = energy.Copy(),
                source = source.Copy(),
                health = health.Copy(),
                speed = speed.Copy(),
                castSpeed = castSpeed.Copy(),
                attack = attack.Copy(),
                defence = defence.Copy()
            };
        }
    }
}