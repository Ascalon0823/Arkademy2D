using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy2D.Core.System
{
    public static class SaveSystem
    {
        public static void SavePlayer()
        {
            var playerSavePath = Path.Combine(Application.persistentDataPath, "player.json");
            GameSystem.PlayerData.LastUpdateDate = DateTime.UtcNow;
            GameSystem.CharacterData.LastUpdateDate = DateTime.UtcNow;
            var playerData = JsonConvert.SerializeObject(GameSystem.PlayerData);
            File.WriteAllText(playerSavePath, playerData);
            Debug.Log($"Player {GameSystem.PlayerData.Id} saved");
        }

        public static Data.PlayerData LoadPlayer()
        {
            var playerSavePath = Path.Combine(Application.persistentDataPath, "player.json");
            if (!File.Exists(playerSavePath)) return null;
            var playerData = JsonConvert.DeserializeObject<Data.PlayerData>(File.ReadAllText(playerSavePath));
            Debug.Log($"Player {playerData.Id} loaded");
            return playerData;
        }
    }
}