using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Common
{
    [Serializable]
    public class Resource : Attribute
    {
        public int currValue;

        public new Resource Copy()
        {
            return new Resource
            {
                currValue = currValue,
                type = type,
                value = value
            };
        }

        public static Resource energy => new() { type = Type.Energy, value = 100, currValue = 100 };
        public static Resource source => new() { type = Type.Source, value = 1000, currValue = 1000 };
        public static Resource health => new() { type = Type.Health, value = 1000, currValue = 1000 };
    }
}