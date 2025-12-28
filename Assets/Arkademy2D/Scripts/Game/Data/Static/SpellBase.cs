using System.Linq;
using Arkademy2D.Game.Behaviours;
using Arkademy2D.Game.Behaviours.Actor;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static
{
    public class SpellBase : StaticData<SpellBase>
    {
        public string spellKey;

        public static SpellBase GetSpellByKey(string spellKey)
        {
            return Library.FirstOrDefault(x => x.Value.spellKey == spellKey).Value;
        }
        public SpellUsage spellUsagePrefab;

        public SpellUsage UseSpell(Caster caster)
        {
            if (!spellUsagePrefab || !caster)
            {
                return null;
            }
            return Instantiate(spellUsagePrefab, caster.transform);
        }
    }
}