using System.Collections.Generic;

namespace Arkademy.Data
{
    public partial class Character
    {
        private static readonly Dictionary<string, string> abbreviation = new Dictionary<string, string>
        {
            { Str, "STR" },
            { Con, "CON" },
            { Dex, "DEX" },
            { Wis, "WIS" },
            { Fai, "FAI" },
            { Cha, "CHA" }
        };

        public static string GetAbbreviation(string character)
        {
            return abbreviation.TryGetValue(character, out var result) ? result : character.Substring(0, 3).ToUpper();
        }

        public const string Lv = "Level";
        public const string XP = "XP";
        public const string AP = "AP";

        public const string Life = "Life";
        public const string Src = "Source";

        public const string Str = "Strength";
        public const string Con = "Constitution";
        public const string Dex = "Dexterity";
        public const string Wis = "Wisdom";
        public const string Fai = "Faith";
        public const string Cha = "Charisma";

        public const string Lck = "Luck";
        public const string MSpd = "Move Speed";
        public const string PhyAtk = "Physical Attack";
        public const string Acc = "Accuracy";
        public const string Kbk = "Knockback";
        public const string CrtChn = "Critical Chance";
        public const string CrtDmg = "Critical Damage";
        public const string Evd = "Evade";
        public const string Stc = "Stance";
        public const string DmgEfc = "Damage Effectiveness";
        public const string PhyRes = "Physical Resistance";
        public const string CstSpd = "Cast Speed";
        public const string SrcEfc = "Source Efficiency";
        public const string CstRan = "Cast Range";
    }
}