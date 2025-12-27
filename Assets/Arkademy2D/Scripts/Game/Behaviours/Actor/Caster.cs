using System.Collections.Generic;
using Arkademy2D.Game.Data.Static;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours.Actor
{
    public class Caster : MonoBehaviour
    {
        public List<string> castKeys;
        public SpellUsage currSpellUsage;
        public bool casting;
        public Energy energy;
        public void Cast(string key)
        {
            if (castKeys == null)
            {
                castKeys = new List<string>();
            }

            if (!castKeys.Contains(key))
            {
                castKeys.Add(key);
            }
        }

        public void EndCast()
        {
            if (castKeys == null) return;
            Debug.Log($"End cast with spell key {string.Join('-',castKeys)}");
            var spellBase = SpellBase.GetSpellByKey(string.Join("", castKeys));
            castKeys = null;
            if (energy && energy.current < energy.max) return;
            currSpellUsage = spellBase.UseSpell(this);
            if (energy) energy.currentFloat = 0f;
        }
    }
}