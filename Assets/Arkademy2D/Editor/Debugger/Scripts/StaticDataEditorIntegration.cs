using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static void CreateNewStaticData<T>(Action<T> postCreationAction = null) where T : StaticData
        {
            var newItem = StaticData<T>.Create();
            if (postCreationAction != null)
            {
                postCreationAction(newItem);
            }

            var savePath = Path.Combine("Assets", "Arkademy2D", "Resources", StaticData<T>.GetResourcePath());
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

        [MenuItem("Static Data/Create/Item/Empty")]
        public static void CreateNewItem()
        {
            CreateNewStaticData<ItemBase>();
        }

        [MenuItem("Static Data/Create/Module/Empty")]
        public static void CreateNewModule()
        {
            CreateNewStaticData<ModuleBase>();
        }
        
        [MenuItem("Static Data/Create/Spell/Empty")]
        public static void CreateNewSpell()
        {
            CreateNewStaticData<SpellBase>();
        }

        [MenuItem("Static Data/Create/New Attribute")]
        public static void CreateNewAttribute()
        {
            CreateNewStaticData<AttributeBase>();
        }

        [MenuItem("Static Data/Create/Character/Empty")]
        public static void CreateNewCharacter()
        {
            CreateNewStaticData<CharacterBase>(x =>
            {
                var last = CharacterBase.Library.LastOrDefault();
                if (last.Value)
                {
                    x.attributeConfigs = last.Value.attributeConfigs.Copy();
                }
            });
        }

        private static List<AttributeConfig> Copy(this List<AttributeConfig> configs)
        {
            return configs.Select(x => x.Copy()).ToList();
        }
    }

    public class CustomAssetModificationProcessor : AssetModificationProcessor
    {
        static string[] OnWillSaveAssets(string[] paths)
        {
            Debug.Log("OnWillSaveAssets was called. Assets being saved:");
            foreach (string path in paths)
            {
                Debug.Log("-" + path);
                var data = AssetDatabase.LoadAssetAtPath<StaticData>(path);
                if (data)
                {
                    var assetName = data.GetItemAssetName();
                    Debug.Log(assetName);
                    AssetDatabase.RenameAsset(path, assetName);
                }
            }

            return paths;
        }
    }
}