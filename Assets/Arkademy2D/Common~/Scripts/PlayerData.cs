using System;
using System.Collections.Generic;

namespace Arkademy2D.Common
{
    public class PlayerData
    {
        public Guid Guid;
        public DateTime CreationTime;
        public DateTime LastUpdateTime;
        public string DisplayName;
        public List<CharacterData> Characters = new List<CharacterData>();
    }
}