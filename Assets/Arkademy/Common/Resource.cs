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
        public static Resource energy => new() { type = Type.Energy };
        public static Resource source => new() { type = Type.Source };
        public static Resource health => new() { type = Type.Health };
    }
}