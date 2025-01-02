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
            DetectionRange
        }

        public static Attribute speed => new() { type = Type.Speed, value = 400 };
        public static Attribute castSpeed => new() { type = Type.CastSpeed, value = 100 };
        public static Attribute attack => new() { type = Type.Attack, value = 100 };
        public static Attribute defence => new() { type = Type.Defence, value = 100 };
        public static Attribute detectionRange => new() { type = Type.DetectionRange, value = 800 };
    }
}