using System;

namespace Arkademy.Data
{
    [Serializable]
    public class AttrDisplay
    {
        public Attribute attribute;
        public int baseValue;
        public float value;
        public float current;
        public int baseCurrent;

        public AttrDisplay(Attribute attribute)
        {
            this.attribute = attribute;
            Update();
        }

        public void Update()
        {
            baseValue = attribute.BaseValue();
            value = attribute.Value();
            current = attribute.Curr();
            baseCurrent = attribute.BaseCurr();
        }
    }
}