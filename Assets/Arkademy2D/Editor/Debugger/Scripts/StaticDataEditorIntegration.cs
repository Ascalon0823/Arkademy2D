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

        [MenuItem("Static Data/Rename all")]
        public static void RenameAll()
        {
            var assets = AssetDatabase.FindAssets("t: StaticData");
            foreach (var asset in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                Debug.Log(path);
                var data = AssetDatabase.LoadAssetAtPath<StaticData>(path);
                var assetName = data.GetItemAssetName();
                Debug.Log(assetName);
                AssetDatabase.RenameAsset(path, assetName);
            }
        }
        [MenuItem("Static Data/Create/New Item")]
        public static void CreateNewItem()
        {
            CreateNewStaticData<ItemBase>();
        }
        [MenuItem("Static Data/Create/New Module")]
        public static void CreateNewModule()
        {
            CreateNewStaticData<ModuleBase>();
        }

        [MenuItem("Static Data/Create/New Usable")] public static void CreateNewUsable()
        {
            CreateNewStaticData<UsableBase>();
        }
        
        [MenuItem("Static Data/Create/New Weapon")]
        public static void CreateNewWeapon()
        {
            CreateNewStaticData<WeaponBase>();
        }

        [MenuItem("Static Data/Create/New Spell")]
        public static void CreateNewSpell()
        {
            CreateNewStaticData<SpellBase>();
        }
    }
}