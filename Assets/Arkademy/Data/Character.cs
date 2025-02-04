using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Arkademy.Data
{
    [Serializable]
    public class Character
    {
        public string displayName;
        public string raceName;

        [JsonIgnore] private Dictionary<Attribute.Type, Attribute> _attributes;

        [JsonIgnore] public Dictionary<Attribute.Type, Attribute> attributes
        {
            get
            {
                if(_attributes == null)SetupAttribute();
                return _attributes;
            }
        }

        public Attribute this[Attribute.Type t]
        {
            get
            {
                if (_attributes == null)
                {
                    SetupAttribute();
                }

                return _attributes.GetValueOrDefault(t, null);
            }
        }

        public void SetCurr(Attribute.Type t, int current)
        {
            var attr = this[t];
            if (attr != null && attr.IsResource())
            {
                attr.current = current;
            }
        }

        public float Get(Attribute.Type t, float defaultValue = 0)
        {
            return this[t]?.Value() ?? defaultValue;
        }

        public int GetBase(Attribute.Type t, int defaultValue = 0)
        {
            return this[t]?.BaseValue() ?? defaultValue;
        }

        public float GetCurr(Attribute.Type t, float defaultValue = 0)
        {
            return this[t]?.Curr() ?? defaultValue;
        }

        public int GetBaseCurr(Attribute.Type t, int defaultValue = 0)
        {
            return this[t]?.BaseCurr() ?? defaultValue;
        }

        private void SetupAttribute()
        {
            _attributes = new Dictionary<Attribute.Type, Attribute>();
            var race = Race.GetRace(raceName);
            foreach (var attr in race.attributes)
            {
                _attributes[attr.type] = attr.Copy();
            }
        }
    }
}