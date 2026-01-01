using System.Linq;

namespace Arkademy2D.Game.Data.Static
{
    public class SpellBase : StaticData<SpellBase>
    {
        public string spellKey;

        public static SpellBase GetSpellByKey(string spellKey)
        {
            return Library.FirstOrDefault(x => x.Value.spellKey == spellKey).Value;
        }
    }
}