using System;
using Arkademy2D.Game.Data.Static;

namespace Arkademy2D.Game.Data.Runtime
{
    [Serializable]
    public class Attribute
    {
        public AttributeConfig config;
        public int rolledValue;
        public int Value => config.defaultValue;
    }
}