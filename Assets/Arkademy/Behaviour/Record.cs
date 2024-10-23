using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arkademy.Templates;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Behaviour
{
    [Serializable]
    public class PlayerRecord
    {
        public static string saveRoot => Path.Combine(Application.persistentDataPath, "Players");
        public string playerName;
        public DateTime CreationTime;
        public DateTime LastPlayed;
        [JsonIgnore] public List<CharacterRecord> characterRecords = new();

        public void Save()
        {
            var playerRoot = Path.Combine(saveRoot, playerName);
            if (!Directory.Exists(playerRoot)) Directory.CreateDirectory(playerRoot);
            var path = Path.Combine(playerRoot, "record.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(this));

            foreach (var character in characterRecords)
            {
                var characterRoot = Path.Combine(playerRoot, "Characters");
                if (!Directory.Exists(characterRoot)) Directory.CreateDirectory(characterRoot);
                var characterRecordPath = Path.Combine(characterRoot, $"{character.characterData.name}.json");
                File.WriteAllText(characterRecordPath, JsonConvert.SerializeObject(character));
            }
        }

        public static List<PlayerRecord> LoadAll()
        {
            Debug.Log($"Attempt to load all save from {saveRoot}");
            var directories = new DirectoryInfo(saveRoot);
            var ret = new List<PlayerRecord>();
            if (!directories.Exists) directories.Create();
            foreach (var directory in directories.GetDirectories())
            {
                ret.Add(Load(directory.FullName));
            }

            return ret.OrderByDescending(x => x.LastPlayed).ToList();
        }

        public static PlayerRecord Load(string playerRootPath)
        {
            var recordPath = Path.Combine(playerRootPath, "record.json");
            var record = JsonConvert.DeserializeObject<PlayerRecord>(File.ReadAllText(recordPath));
            var characterRootPath = new DirectoryInfo(Path.Combine(playerRootPath, "Characters"));
            if (!characterRootPath.Exists) return record;
            foreach (var file in characterRootPath.GetFiles())
            {
                var charaRecord = JsonConvert.DeserializeObject<CharacterRecord>(File.ReadAllText(file.FullName));
                var templateName = charaRecord.characterData.templateName;
                var template = Resources.Load<CharacterTemplate>(templateName);
                charaRecord.characterData.UpdateFieldsBy(template.templateData);
                record.characterRecords.Add(charaRecord);
            }

            record.characterRecords = record.characterRecords.OrderByDescending(x => x.LastPlayed).ToList();
            record.Save();
            return record;
        }
    }

    [Serializable]
    public class CharacterRecord
    {
        public Data.Character characterData;
        public DateTime CreationTime;
        public DateTime LastPlayed;
    }
}