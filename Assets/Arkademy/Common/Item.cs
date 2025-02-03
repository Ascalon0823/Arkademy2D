using System;

namespace Arkademy.Common
{
    [Serializable]
    public class Item
    {
        public string name;
        public string displayName;
        public Offense offense;
        public Defense defense;
    }
    

    [Serializable]
    public class Offense
    {
        public int attack;
        public int range;
        public int speed;
        public bool isRange;
    }
    
    [Serializable]
    public class Defense
    {
        public int defense;
    }
}