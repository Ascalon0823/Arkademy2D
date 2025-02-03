using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Character
    {
        public string displayName;
        public string raceName;
        [JsonIgnore] [SerializeReference] private Race race;
        [JsonIgnore] private Dictionary<Attribute.Type, Attribute> _attrMap;

        private void SetupMap()
        {
            if (race == null) return;
            foreach (var attr in race.attributes)
            {
                _attrMap[attr.type] = attr;
            }
        }
        public Attribute this[Attribute.Type t]
        {
            get
            {
                if(_attrMap==null)SetupMap();
                return _attrMap[t];
            }
        }
    }
}