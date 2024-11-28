using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Templates
{
    [CreateAssetMenu(fileName = "New Modifier Affix", menuName = "Template/Affix/Modifier Affix", order = 0)]
    public class ModifierAffixTemplate : AffixTemplate<ModifierEffect>
    {
        public override Affix GetAffix(float value, EquipmentSlot.Category category = EquipmentSlot.Category.MainHand,
            int rarity = 0)
        {
            var affix = base.GetAffix(value, category, rarity);
            affix.effects ??= new List<Data.Effect>();
            var e = effect.Copy();
            foreach (var modifier in e.modifiers)
            {
                var categoryConfig = config.FirstOrDefault(x => (x.categories & category) == category);
                var rarityConfig = categoryConfig?.rarities.FirstOrDefault(x => (x.rarity == rarity));
                if (rarityConfig != null)
                    modifier.Value = (long)Mathf.Lerp(rarityConfig.minValue, rarityConfig.maxValue, value);
            }

            affix.effects.Add(e);
            return affix;
        }
    }
}