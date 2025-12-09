using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Arkademy2D.Core.Data
{
    [Serializable]
    public class PlayerData
    {
        [JsonProperty] public Guid Id { get; private set; } = Guid.NewGuid();
        [JsonProperty] public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
        public DateTime LastUpdateDate { get; set; }  = DateTime.UtcNow;
        public string DisplayName { get; set; }

        [JsonProperty] public List<CharacterData> Characters { get; private set; } = new List<CharacterData>();
    }
}