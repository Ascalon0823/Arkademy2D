using System.Collections.Generic;
using Arkademy2D.Game.Data.Runtime;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Arkademy2D.Game.Data.Static.Usable
{
    [CreateAssetMenu(fileName = "Heal Usable Effect", menuName = "Static/Usable/HealEffect")]
    public class HealUsableDefinition:UsableEffectDefinition
    {
        public AttributeBase healAmountAttribute;
        public AttributeBase useTimeAttribute;
        public override IReadOnlyList<AttributeBase> RequiredAttributes => new []{
            healAmountAttribute, useTimeAttribute};

        public override Usage GetUsage(UseContext ctx)
        {
            return new Usage
            {
                beginTime = Time.timeSinceLevelLoad
            };
        }
    }
}