using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        }

        [JsonIgnore] public Dictionary<Modifier.Category, List<Modifier>> Modifiers = new();

        public void AddMod(Modifier modifier)
        {
            if (!Modifiers.TryGetValue(modifier.category, out var modifiers))
            {
                modifiers = new List<Modifier>();
                Modifiers[modifier.category] = modifiers;
            }

            modifiers.Add(modifier);
        }

        public void RemoveMod(Modifier modifier)
        {
            if (Modifiers.TryGetValue(modifier.category, out var modifiers))
            {
                modifiers.Remove(modifier);
            }
        }
    }
}