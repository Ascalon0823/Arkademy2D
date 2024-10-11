using System;
using UnityEngine;

namespace Arkademy.Data
{
    [CreateAssetMenu(fileName = "New Equip Attr Affix", menuName = "Data/Affix/Equipment Attr", order = 0)]
    public class EquipmentAttrAffix : AffixBase
    {
        public EquipmentData.Attr.Category attrCategory;

        protected override AffixData.TargetCategories GetCategories()
        {
            return new AffixData.TargetCategories
            {
                category = attrCategory
            };
        }
    }
}