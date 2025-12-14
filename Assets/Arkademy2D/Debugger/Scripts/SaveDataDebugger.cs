using Arkademy2D.Core.Data;
using Arkademy2D.Core.Models;
using Arkademy2D.Core.System;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy2D.Debugger
{
    [CreateAssetMenu(fileName = "SaveDataDebugger", menuName = "Debugger/SaveDebugger")]
    public class SaveDataDebugger : ScriptableObject
    {
        [TextArea(3, 100000)] public string playerDataJson;

        public string playerDataKey;

        [ContextMenu("New")]
        public void NewPlayer()
        {
            var data = new PlayerData
            {
                PlayerModel = new Player()
            };
            playerDataJson = JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        [ContextMenu("Load")]
        public void LoadPlayer()
        {
            playerDataJson = JsonConvert.SerializeObject(SaveSystem.LoadPlayer(playerDataKey), Formatting.Indented);
        }

        [ContextMenu("Save")]
        public void SavePlayer()
        {
            SaveSystem.SavePlayer(JsonConvert.DeserializeObject<PlayerData>(playerDataJson));
        }
    }
}