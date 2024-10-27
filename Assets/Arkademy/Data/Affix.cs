using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Templates;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Affix
    {
        public string templateName;
        [JsonIgnore]public List<Effect> effects;
        [Range(0f, 1f)] public float value;

        public Affix Copy()
        {
            return new Affix
            {
                templateName = templateName,
                effects = effects?.ToList(),
                value = value
            };
        }

        public void OnEquippedTo(Data.Character character, Data.Equipment equipment)
        {
            var template = Resources.Load<AffixTemplate>(templateName);
            var affix = template.GetAffix(value, equipment.slotCategory, equipment.rarity);
            foreach (var effect in affix.effects)
            {
                effect.AppliedTo(character);
            }
        }
    }
}