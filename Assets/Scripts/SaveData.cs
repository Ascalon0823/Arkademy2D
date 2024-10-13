using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arkademy.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy
{
    [Serializable]
    public class SaveData
    {
        [Serializable]
        public class CharacterPlayRecord
        {
            public int characterDBIdx;
            public int numOfGamePlayed;
        }

        [Serializable]
        public class CharacterRecord
        {
            public Character character;
            public DateTime CreationDate;
            public DateTime LastPlayed;
        }
        [Serializable]
        public class PlayerRecord
        {
            public List<CharacterRecord> characters;
        }
        public static SaveData current
        {
            get
            {
                if (_current != null) return _current;
                if (TryLoadSaveData(out _current)) return _current;
                _current = new SaveData();
                return _current;
            }
        }

        private static string saveDataPath => Path.Combine(Application.persistentDataPath, "save.data");
        private static SaveData _current;
        public int numOfGamesPlayed;
        public List<CharacterPlayRecord> characterPlayRecords = new List<CharacterPlayRecord>();
        public void Save()
        {
            Debug.Log($"Saved to {saveDataPath}");
            File.WriteAllText(saveDataPath, JsonConvert.SerializeObject(this));
        }

        public void AddCharacterPlayRecordAndSave(int charaDbIdx)
        {
            var record = characterPlayRecords.FirstOrDefault(x => x.characterDBIdx == charaDbIdx);
            if (record == null)
            {
                record = new CharacterPlayRecord
                {
                    characterDBIdx = charaDbIdx
                };
                characterPlayRecords.Add(record);
            }
            record.numOfGamePlayed++;
            numOfGamesPlayed++;
            Save();
        }

        public bool TryGetCharacterPlayRecord(int dbIdx, out CharacterPlayRecord record)
        {
            record = characterPlayRecords.FirstOrDefault(x => x.characterDBIdx == dbIdx);
            return record != null;
        }

        private static bool TryLoadSaveData(out SaveData data)
        {
            data = null;
            if (!File.Exists(saveDataPath)) return false;
            var json = File.ReadAllText(saveDataPath);
            data = JsonConvert.DeserializeObject<SaveData>(json);
            return data != null;
        }
    }
}