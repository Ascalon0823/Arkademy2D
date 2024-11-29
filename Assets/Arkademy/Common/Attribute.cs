using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Common
{
    [Serializable]
    public partial class Attribute : Progression
    {
        public Type type;
        public int value;
    }

    [Serializable]
    public class Attributes
    {
        [SerializeField] private List<Attribute> attributes = new List<Attribute>();
        private Dictionary<Attribute.Type, Attribute> _attributesByType = new Dictionary<Attribute.Type, Attribute>();
        public Attribute this[Attribute.Type key] => _attributesByType.GetValueOrDefault(key);
    }
}