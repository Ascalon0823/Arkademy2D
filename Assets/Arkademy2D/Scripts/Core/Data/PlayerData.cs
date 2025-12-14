using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Arkademy2D.Core.Data
{
    [Serializable]
    public class PlayerData
    {
        public Models.Player PlayerModel;
        [JsonProperty] public List<CharacterData> Characters { get; private set; } = new List<CharacterData>();
    }
}