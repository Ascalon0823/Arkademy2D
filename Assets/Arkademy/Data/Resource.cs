using System.Collections.Generic;

namespace Arkademy.Data
{
    public partial class Attribute
    {
        private static readonly HashSet<Type> _resTypes = new()
        {
            Type.Life
        };
        public int current;

        public bool IsResource()
        {
            return _resTypes.Contains(type);
        }
    }
}