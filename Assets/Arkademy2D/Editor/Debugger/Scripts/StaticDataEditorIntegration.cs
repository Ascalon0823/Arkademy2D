using System.IO;
using Arkademy2D.Game.Data.Static;
using Arkademy2D.Game.Data.Static.Academic;
using Arkademy2D.Game.Data.Static.Item;
using Arkademy2D.Game.Data.Static.Usable;
using UnityEditor;
using UnityEngine;

namespace Arkademy2D.Editor.Debugger
{
    public static class StaticDataEditorIntegration
    {
        private static void CreateNewStaticData<T>() where T : StaticData
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

        [MenuItem("Create/New Usable")] public static void CreateNewUsable()
        {
            CreateNewStaticData<UsableBase>();
        }
        
        [MenuItem("Create/New Weapon")]
        public static void CreateNewWeapon()
        {
            CreateNewStaticData<WeaponBase>();
        }
    }
}