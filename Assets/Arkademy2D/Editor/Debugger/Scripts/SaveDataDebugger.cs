using System.IO;
using System.Linq;
using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Core.Models;
using Arkademy2D.Game.Data.Static;
using Arkademy2D.Game.Data.Static.Academic;
using Arkademy2D.Game.Data.Static.Item;
using Arkademy2D.Game.Data.Static.Usable;
using Arkademy2D.Game.System;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Arkademy2D.Editor.Debugger
{
    [CreateAssetMenu(fileName = "SaveDataDebugger", menuName = "Debugger/SaveDebugger")]
    public class SaveDataDebugger : ScriptableObject
    {
        [TextArea(3, 100000)] public string playerModelJson;

        public string PlayerModelJson
        {
            get => playerModelJson;
            set
            {
                playerModelJson = value;
                ForceUpdateInspector();
            }
        }
        public string playerModelKey;

        private void ForceUpdateInspector()
        {
            EditorUtility.SetDirty(this);
        }
        [ContextMenu("New")]
        public void NewPlayer()
        {
            PlayerModelJson = JsonConvert.SerializeObject(new Player(), Formatting.Indented);
        }
        
        [ContextMenu("Load all")]
        public void LoadAllPlayers()
        {
            var players = SaveSystem.LoadAllPlayers();
            foreach (var player in players)
            {
                Debug.Log(player.Key);
            }
        }

        [ContextMenu("Load")]
        public void LoadPlayer()
        {
            PlayerModelJson = JsonConvert.SerializeObject(SaveSystem.LoadPlayer(playerModelKey), Formatting.Indented);
            ForceUpdateInspector();
        }

        [ContextMenu("Save")]
        public void SavePlayer()
        {
            SaveSystem.SavePlayer(JsonConvert.DeserializeObject<Player>(PlayerModelJson));
        }

        public string selectCharacterKey;
        public ItemBase itemBaseToAdd;

        [ContextMenu("Add item to character")]
        public void AddItemToCharacter()
        {
            var playerModel= JsonConvert.DeserializeObject<Player>(PlayerModelJson);
            var chara = playerModel.Characters.FirstOrDefault(x => x.Id.ToString() == selectCharacterKey);
            if (chara == null)
            {
                Debug.Log($"Character {selectCharacterKey} not found");
                return;
            }

            if (!itemBaseToAdd)
            {
                Debug.Log("No item base");
                return;
            }
            chara.Items.Add(new Item{ItemBaseId = int.Parse(itemBaseToAdd.Id)});
            PlayerModelJson =  JsonConvert.SerializeObject(playerModel, Formatting.Indented);
        }
    }
}