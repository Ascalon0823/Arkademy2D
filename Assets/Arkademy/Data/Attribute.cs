using System;

namespace Arkademy.Data
{
    [Serializable]
    public struct Attribute
    {
        public static Attribute strength => new() { key = "Strength", value = 10 };
        public string key;
        public int value;
    }
}