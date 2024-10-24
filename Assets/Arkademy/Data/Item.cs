using System;
using System.Collections.Generic;

namespace Arkademy.Data
{
    [Serializable]
    public class Item
    {
        public string templateName;
        public string name;
        public int stackLimit;
        public List<Affix> inventoryAffix;
        public List<Requirement> requirements;
    }
}