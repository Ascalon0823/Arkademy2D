using System;
using UnityEngine;

namespace Arkademy.Data.Deprecate
{
    [CreateAssetMenu(fileName = "New Chara Attr Affix Base", menuName = "Data/Affix/Character Attr", order = 0)]
    public class CharacterAttrAffix : AffixBase
    {
        public CharacterData.Attr.Category attrCategory;
        
        protected override AffixData.TargetCategories GetCategories()
        {
            return new AffixData.TargetCategories
            {
                category = AffixData.Category.CharacterAttrBoost,
                categoryValue = (int)attrCategory
            };
        }
    }
}