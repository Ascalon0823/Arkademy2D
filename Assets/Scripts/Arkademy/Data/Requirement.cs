using System;

namespace Arkademy.Data
{
    [Serializable]
    public struct Requirement
    {
        public Attribute attribute;
        public int minValue;
    }
}