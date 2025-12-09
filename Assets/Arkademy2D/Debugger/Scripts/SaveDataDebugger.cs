using Arkademy2D.Core.Data;
using Arkademy2D.Core.System;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy2D.Debugger
{
    [CreateAssetMenu(fileName = "SaveDataDebugger", menuName = "Debugger/SaveDebugger")]
    public class SaveDataDebugger : ScriptableObject
    {
        [TextArea(3,100000)]
        public string playerDataJson;

        [ContextMenu("Load")]
        public void LoadPlayer()
        {
            playerDataJson = SaveSystem.ReadPlayerDataJson();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("Save")]
        public void SavePlayer()
        {
            SaveSystem.WritePlayerDataJson(playerDataJson);
        }
    }
}
