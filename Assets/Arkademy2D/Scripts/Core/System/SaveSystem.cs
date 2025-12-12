using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy2D.Core.System
{
    public static class SaveSystem
    {
        public static string PlayerSavePath => Path.Combine(Application.persistentDataPath, "player.json");
        public static void SavePlayer()
        {
            GameSystem.PlayerData.LastUpdateDate = DateTime.UtcNow;
            GameSystem.CharacterData.LastUpdateDate = DateTime.UtcNow;
            var playerDataJson = JsonConvert.SerializeObject(GameSystem.PlayerData, Formatting.Indented);
            WritePlayerDataJson(playerDataJson);
        }

        public static Data.PlayerData LoadPlayer()
        {
            if (!File.Exists(PlayerSavePath)) return null;
            var playerData = JsonConvert.DeserializeObject<Data.PlayerData>(ReadPlayerDataJson());
            if(playerData != null)
                Debug.Log($"Player {playerData.Id} loaded");
            return playerData;
        }

        public static void WritePlayerDataJson(string playerDataJson)
        {
            File.WriteAllText(PlayerSavePath, playerDataJson);
            Debug.Log($"Player {GameSystem.PlayerData.Id} saved");
        }

        public static string ReadPlayerDataJson()
        {
            return File.ReadAllText(PlayerSavePath);
        }
    }
}