using System;

namespace Arkademy2D.Common
{
    [Serializable]
    public record PlayerData
    {
        public Guid Guid;
        public DateTime CreationTime;
        public DateTime LastUpdateTime;
        public string displayName;
    }
}