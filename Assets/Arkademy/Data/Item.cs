using System;
using System.Collections.Generic;

namespace Arkademy.Data
{
    [Serializable]
    public class Item
    {
        public string templateName;
        public string name;
        public int rarity;
        public int stackLimit;
        public int stack;

        public virtual bool Valid()
        {
            return !string.IsNullOrEmpty(templateName);
        }

        public virtual Item Copy()
        {
            return new Item
            {
                templateName = templateName,
                name = name,
                rarity = rarity,
                stackLimit = stackLimit,
                stack = stack
            };
        }
    }
}