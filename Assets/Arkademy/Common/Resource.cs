using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Common
{
    [Serializable]
    public class Resource : Attribute
    {
        public int currValue;

        public static Resource energy => new() { type = Type.Energy };
    }
}