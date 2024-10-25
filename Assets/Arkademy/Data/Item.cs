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
    }
}