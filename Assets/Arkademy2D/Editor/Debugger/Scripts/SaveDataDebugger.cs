using System.IO;
using System.Linq;
using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Core.Models;
using Arkademy2D.Game.Data.Static;
using Arkademy2D.Game.Data.Static.Academic;
using Arkademy2D.Game.Data.Static.Item;
using Arkademy2D.Game.System;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Arkademy2D.Editor.Debugger
{
    [CreateAssetMenu(fileName = "SaveDataDebugger", menuName = "Debugger/SaveDebugger")]
    public class SaveDataDebugger : ScriptableObject
    {
        [TextArea(3, 100000)] public string playerDataJson;

        public string PlayerDataJson
        {
            get => playerDataJson;
            set
            {
                playerDataJson = value;
                ForceUpdateInspector();
            }
        }
        public string playerDataKey;

        private void ForceUpdateInspector()
        {
            EditorUtility.SetDirty(this);
        }
        [ContextMenu("New")]
        public void NewPlayer()
        {
            var data = new PlayerData
            {
                PlayerModel = new Player()
            };
            PlayerDataJson = JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        
        [ContextMenu("Load all")]
        public void LoadAllPlayers()
        {
            var players = SaveSystem.LoadAllPlayers();
            foreach (var player in players)
            {
                Debug.Log(player.PlayerModel.Key);
            }
        }

        [ContextMenu("Load")]
        public void LoadPlayer()
        {
            PlayerDataJson = JsonConvert.SerializeObject(SaveSystem.LoadPlayer(playerDataKey), Formatting.Indented);
            ForceUpdateInspector();
        }

        [ContextMenu("Save")]
        public void SavePlayer()
        {
            SaveSystem.SavePlayer(JsonConvert.DeserializeObject<PlayerData>(PlayerDataJson));
        }

        public string selectCharacterKey;
        public ItemBase itemBaseToAdd;

        [ContextMenu("Add item to character")]
        public void AddItemToCharacter()
        {
            var playerData = JsonConvert.DeserializeObject<PlayerData>(PlayerDataJson);
            var chara = playerData.PlayerModel.Characters.FirstOrDefault(x => x.Id.ToString() == selectCharacterKey);
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
            PlayerDataJson =  JsonConvert.SerializeObject(playerData, Formatting.Indented);
        }

        private static void CreateNewStaticData<T>() where T : StaticData<T>
        {
            var newItem = StaticData<T>.Create();
            var savePath = Path.Combine("Assets","Arkademy2D","Resources", StaticData<T>.GetResourcePath());
            var assetName = $"{newItem.name}.asset";
            AssetDatabase.CreateAsset(newItem, Path.Combine(savePath, assetName));
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newItem; 
        }
        [MenuItem("Create/New Item")]
        public static void CreateNewItem()
        {
            CreateNewStaticData<ItemBase>();
        }
        [MenuItem("Create/New Module")]
        public static void CreateNewModule()
        {
            CreateNewStaticData<ModuleBase>();
        }
    }
}