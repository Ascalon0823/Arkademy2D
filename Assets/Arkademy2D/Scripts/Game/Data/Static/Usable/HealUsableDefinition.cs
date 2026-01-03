using System.Collections.Generic;
using Arkademy2D.Game.Data.Runtime;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Usable
{
    [CreateAssetMenu(fileName = "Heal Usable Effect", menuName = "Static/Usable/HealEffect")]
    public class HealUsableDefinition:UsableEffectDefinition
    {
        public AttributeBase healAmountAttribute;
        public AttributeBase useTimeAttribute;
        public override IReadOnlyList<AttributeBase> RequiredAttributes => new []{
            healAmountAttribute, useTimeAttribute};
    }
}