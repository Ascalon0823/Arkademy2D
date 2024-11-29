namespace Arkademy.Common
{
    public partial class Attribute
    {
        public enum Type
        {
            None,
            Energy,
            Speed
        }

        public static Attribute speed =>new(){type = Type.Speed};
    }
}