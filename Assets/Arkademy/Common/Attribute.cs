using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Common
{
    [Serializable]
    public partial class Attribute
    {
        public Type type;
        public int value;

        public Attribute Copy()
        {
            return new Attribute
            {
                type = type,
                value = value
            };
        }
    }
}