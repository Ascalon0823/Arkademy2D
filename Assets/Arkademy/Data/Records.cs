using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arkademy.Data
{
    [Serializable]
    public class PlayerRecord
    {
        public List<CharacterRecord> characterRecords = new List<CharacterRecord>();

        public void Save()
        {
            var data = JsonConvert.SerializeObject(this);
            PlayerPrefs.SetString("PlayerData", data);
        }

        public static PlayerRecord LoadOrNew()
        {
            var data = PlayerPrefs.GetString("PlayerData", "");
            if (string.IsNullOrEmpty(data))
            {
                var record = new PlayerRecord();
                record.Save();
                return record;
            }

            return JsonConvert.DeserializeObject<PlayerRecord>(data);
        }
    }

    [Serializable]
    public class CharacterRecord
    {
        public DateTime CreationTime;
        public DateTime LastPlayed;
        public TimeSpan PlayedDuration;
        public Character character;
        public int clearedDifficulty;
    }
}