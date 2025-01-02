using System;
using UnityEngine;

namespace Arkademy.Common
{
    [Serializable]
    public class Character
    {
        public string displayName;
        public string raceName;
        public Resource energy;
        public Resource source;
        public Resource health;
        public Attribute speed;
        public Attribute castSpeed;
        public Attribute attack;
        public Attribute defence;
        public Attribute detectionRange;
    }
}