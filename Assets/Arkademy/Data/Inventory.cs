using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Inventory
    {
        public List<Item> items = new List<Item>();
        public int capacity;

        public Inventory Copy()
        {
            return new Inventory
            {
                capacity = capacity,
                items = items.Select(x => x.Copy()).ToList()
            };
        }
    }
}