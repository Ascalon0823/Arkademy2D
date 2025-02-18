using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Data
{
    public partial class Attribute
    {
        [Serializable]
        public class Modifier
        {
            public enum Category
            {
                Addition,
                Multiplication
            }

            public Type attrType;
            public Category category;
            public int value;
            public Modifier Copy()
            {
                return new Modifier
                {
                    attrType = attrType,
                    category = category,
                    value = value
                };
            }
        }

        [JsonIgnore] public Dictionary<Modifier.Category, List<Modifier>> Modifiers = new();

        public void AddMod(Modifier modifier, bool adjustCurr = true)
        {
            var ratio = 1f;
            if (IsResource() && adjustCurr)
            {
                ratio = current * 1f / BaseValue();
            }
            if (!Modifiers.TryGetValue(modifier.category, out var modifiers))
            {
                modifiers = new List<Modifier>();
                Modifiers[modifier.category] = modifiers;
            }

            modifiers.Add(modifier);
            if (IsResource() && adjustCurr)
            {
                current = Mathf.RoundToInt(ratio * BaseValue());
            }
        }
        public void RemoveMod(Modifier modifier, bool adjustCurr = true)
        {
            var ratio = 1f;
            if (IsResource() && adjustCurr)
            {
                ratio = current * 1f / BaseValue();
            }
            if (Modifiers.TryGetValue(modifier.category, out var modifiers))
            {
                modifiers.Remove(modifier);
            }
            if (IsResource() && adjustCurr)
            {
                current = Mathf.RoundToInt(ratio * BaseValue());
            }
        }

        
    }
}