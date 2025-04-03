using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
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
        public CharacterRecord[] characters;
    }

    [Serializable]
    public class CharacterRecord
    {
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
                    .Where(x=>x?.character!=null)
                    .ToList() ?? new List<Data.CharacterRecord>()
            };
        }

        public static PlayerRecord ToServerPlayerRecord(this Data.PlayerRecord playerRecord)
        {
            return new PlayerRecord
            {
                characters = playerRecord.characterRecords.Select(x => x.ToServerCharacterRecord()).ToArray()
            };
        }

        public static CharacterRecord ToServerCharacterRecord(this Data.CharacterRecord characterRecord)
        {
            return new CharacterRecord
            {
                LastPlayedTime = characterRecord.LastPlayed,
                CreationTime = characterRecord.CreationTime,
                Data = JToken.FromObject(characterRecord)
            };
        }

        public static Data.CharacterRecord ToCharacterRecordData(this CharacterRecord characterRecord)
        {
            Debug.Log(characterRecord.Data);
            return JsonConvert.DeserializeObject<Data.CharacterRecord>(characterRecord.Data.ToString(), new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTH:mm:ssK"
            });
        }
    }
}