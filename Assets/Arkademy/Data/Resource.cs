using System;

namespace Arkademy.Data
{
    [Serializable]
    public class Resource
    {
        public Attribute maxValue;
        public Field currValue;

        public Resource(Attribute maxValue)
        {
            this.maxValue = maxValue;
            currValue = new Field(maxValue.key, maxValue.GetValue());
        }
    }
}