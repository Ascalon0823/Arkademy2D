using System;

namespace Arkademy2D.Game.Data.Static
{
    [Serializable]
    public class AttributeConfig
    {
        public AttributeBase @base;
        public int defaultValue;

        public AttributeConfig Copy()
        {
            return new AttributeConfig{@base =  @base, defaultValue = defaultValue};
        }
    }
    public class AttributeBase : StaticData<AttributeBase>
    {
        public string shortName;
    }
}