using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy
{
    [Serializable]
    public class SaveData
    {
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

        public void Save()
        {
            File.WriteAllText(saveDataPath, JsonConvert.SerializeObject(this));
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