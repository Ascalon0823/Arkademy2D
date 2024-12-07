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

    [Serializable]
    public class Attributes
    {
        [SerializeField] private List<Attribute> attributes = new List<Attribute>();
        private Dictionary<Attribute.Type, Attribute> _attributesByType = new Dictionary<Attribute.Type, Attribute>();
        public Attribute this[Attribute.Type key] => _attributesByType.GetValueOrDefault(key);
    }
}