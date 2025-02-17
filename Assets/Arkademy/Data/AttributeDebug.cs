using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<Attribute.Modifier> modifiers;

        public AttrDisplay(Attribute attribute)
        {
            this.attribute = attribute;
            modifiers = this.attribute.Modifiers.ToList().SelectMany(x => x.Value).ToList();
            Update();
        }

        public void Update()
        {
            baseValue = attribute.BaseValue();
            value = attribute.Value();
            current = attribute.Curr();
            baseCurrent = attribute.BaseCurr();
            modifiers = this.attribute.Modifiers.ToList().SelectMany(x => x.Value).ToList();
        }
    }
}