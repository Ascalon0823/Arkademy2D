using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static
{
    public abstract class StaticData : ScriptableObject
    {
        public int id;
        public string displayName;
        
        public abstract string GetItemAssetName();
    }
    public abstract class StaticData<T> : StaticData where T : StaticData
    {
        public static string GetResourcePath()
        {
            return $"Static/{typeof(T).Name}";
        }
        public static T Get(int id)
        {
            return Library.TryGetValue(id, out var result) ? result : Library.FirstOrDefault().Value;
        }
        public static Dictionary<int, T> Library
        {
            get
            {
                if (_library == null)
                {
                    _library = LoadAll();
                }
                return _library;
            }
        }
        private static Dictionary<int, T> _library;
        private static Dictionary<int, T> LoadAll()
        {
            var lib = new Dictionary<int, T>();
            foreach (var item in Resources.LoadAll<T>(GetResourcePath()))
            {
                lib.Add(item.id, item);
            }
            return lib;
        }

        private static void ReloadAll()
        {
            _library = LoadAll();
        }
        
        public static T Create()
        {
            ReloadAll();
            var item = CreateInstance<T>();
            item.id = Library.Count + 1;
            item.name = item.GetItemAssetName();
            return item;
        }

        public override string GetItemAssetName()
        {
            var actualDisplayName = string.IsNullOrWhiteSpace(displayName) ? "Unknown" : displayName; 
            return $"{typeof(T).Name}_{id:000000}_{actualDisplayName}";
        }
    }
}