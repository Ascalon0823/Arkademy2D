using System.Collections;
using System.Collections.Generic;
using Arkademy;
using UnityEngine;

namespace Arkademy
{
    public class Spell : MonoBehaviour
    {
        public CharacterBehaviour user;
        public string spellKey;
        public string spellName;
        public Sprite uiIcon;
        public int instanceId;
        public enum Phase
        {
            Begin,
            Update,
            End
        }

        public struct SpellUsage
        {
            public Phase Phase;
            public Vector2 Direction;
        }

        public virtual bool OnUse(SpellUsage usage)
        {
            return true;
        }
    }

}
