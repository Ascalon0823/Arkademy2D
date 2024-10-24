using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public partial class Character
    {
        [HideInInspector] public string templateName;
        public string name;
        public List<EquipmentSlot> slots;

        public List<Attribute> attributes;

        private Dictionary<string, Attribute> _attrCache = new Dictionary<string, Attribute>();

        private void BuildAttrCache()
        {
            foreach (var attr in attributes)
            {
                _attrCache[attr.key] = attr;
            }
        }

        public bool TryGetAttr(string attrName, out Attribute attribute)
        {
            if (!_attrCache.Any()) BuildAttrCache();
            return _attrCache.TryGetValue(attrName, out attribute);
        }

        public void UpdateFieldsBy(Character other)
        {
            if (string.IsNullOrEmpty(templateName)) templateName = other.templateName;
            attributes ??= new List<Attribute>();
            var hash = attributes.Select(x => x.key).ToHashSet();
            attributes.AddRange(other.attributes.Where(x => !hash.Contains(x.key)).Select(x => x.Copy()));
        }

        public Character Copy()
        {
            return new Character
            {
                templateName = templateName,
                name = name,
                slots = slots.ToList(),
                attributes = attributes.Select(x => x.Copy()).ToList(),
            };
        }
    }
}