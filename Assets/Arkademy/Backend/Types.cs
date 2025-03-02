using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Serialization;

namespace Arkademy.Backend
{
    [Serializable]
    public class User
    {
        public string id;
        public string username;
        public PlayerRecord playerRecord;
    }

    [Serializable]
    public class PlayerRecord
    {
        public DateTime CreationTime;
        public DateTime LastPlayedTime;
        public CharacterRecord[] characters;
    }

    [Serializable]
    public class CharacterRecord
    {
        public string displayName;
        public DateTime CreationTime;
        public DateTime LastPlayedTime;
        public JToken Data;
    }

    public static class Extensions
    {
        public static Data.PlayerRecord ToPlayerRecordData(this PlayerRecord playerRecord)
        {
            return new Data.PlayerRecord
            {
                characterRecords = playerRecord.characters?
                    .Select(x => x.ToCharacterRecordData())
                    .ToList() ?? new List<Data.CharacterRecord>()
            };
        }

        public static Data.CharacterRecord ToCharacterRecordData(this CharacterRecord characterRecord)
        {
            return new Data.CharacterRecord
            {
                character = characterRecord.Data.ToObject<Character>(),
                clearedDifficulty = characterRecord.Data["clearedDifficulty"]?.ToObject<int>() ?? 0,
                CreationTime = characterRecord.CreationTime,
                LastPlayed = characterRecord.LastPlayedTime
            };
        }
    }
}