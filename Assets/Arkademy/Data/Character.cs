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
        public Progression progression;
        public Growth growth;
        public Resources resources;
        public ReactiveFields locomotion;
        public ReactiveFields offensive;
        public ReactiveFields defensive;
        public ReactiveFields casting;
        public ReactiveFields common;
        public List<EquipmentSlot> slots;


        public void UpdateFieldsBy(Character other)
        {
            if (string.IsNullOrEmpty(templateName)) templateName = other.templateName;
            progression ??= other.progression.Copy();
            progression.UpdateFieldsBy(other.progression);
            growth ??= other.growth.Copy();
            growth.Origin.UpdateFieldsBy(other.growth.Origin);
            resources ??= other.resources.Copy();
            resources.Max.UpdateFieldsBy(other.resources.Max);
            locomotion ??= other.locomotion.Copy();
            locomotion.UpdateFieldsBy(other.locomotion);
            offensive ??= other.offensive.Copy();
            offensive.UpdateFieldsBy(other.offensive);
            defensive ??= other.defensive.Copy();
            defensive.UpdateFieldsBy(other.defensive);
            casting ??= other.casting.Copy();
            casting.UpdateFieldsBy(other.casting);
            common ??= other.common.Copy();
            common.UpdateFieldsBy(other.common);
        }

        public Character Copy()
        {
            return new Character
            {
                templateName = templateName,
                name = name,
                progression = progression.Copy(),
                growth = growth.Copy(),
                resources = resources.Copy(),
                locomotion = locomotion.Copy(),
                offensive = offensive.Copy(),
                defensive = defensive.Copy(),
                casting = casting.Copy(),
                common = common.Copy(),
                slots = slots.ToList()
            };
        }
    }
}