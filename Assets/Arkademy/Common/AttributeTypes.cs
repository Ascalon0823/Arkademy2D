namespace Arkademy.Common
{
    public partial class Attribute
    {
        public enum Type
        {
            None,
            Energy,
            Health,
            Source,
            Speed,
            CastSpeed,
            Attack,
            Defence,
            
        }

        public static Attribute speed =>new(){type = Type.Speed};
        public static Attribute castSpeed =>new(){type = Type.CastSpeed};
        public static Attribute attack => new() { type = Type.Attack };
        public static Attribute defence => new(){type = Type.Defence};
    }
}