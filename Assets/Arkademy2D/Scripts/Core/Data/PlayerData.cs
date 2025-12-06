using System;

namespace Arkademy2D.Core.Data
{
    public class PlayerData
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
        public DateTime LastUpdateDate { get; set; }
        public string DisplayName { get; set; }
    }
}