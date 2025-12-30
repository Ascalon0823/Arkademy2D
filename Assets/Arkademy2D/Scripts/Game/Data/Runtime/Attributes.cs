using System.Collections.Generic;
using Arkademy2D.Game.Data.Static;
using UnityEngine;

namespace Arkademy2D.Game.Data.Runtime
{
    public class Attributes
    {
        private Dictionary<Attribute,int> _attributes = new Dictionary<Attribute, int>();
        public void Add(int key, int value)
        {
            if (!Attribute.Library.TryGetValue(key, out var attr))
            {
                Debug.LogError($"Attribute {key} not found");
                return;
            }
            _attributes.Add(attr, value);
        }
        
        public void Clear()
        {
            _attributes.Clear();
        }

        public bool TryGetValue(int key, out int value)
        {
            if (!Attribute.Library.TryGetValue(key, out var attr))
            {
                Debug.LogError($"Attribute {key} not found");
                value = 0;
                return false;
            }
            return _attributes.TryGetValue(attr, out value);
        }
        
        public bool TryGetValue(Attribute attr, out int value)
        {
            return _attributes.TryGetValue(attr, out value);
        }
    }
}