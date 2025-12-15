using System.Collections.Generic;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static
{
    public abstract class StaticData : ScriptableObject
    {
        public int id;
    }
    public abstract class StaticData<T> : StaticData where T : StaticData
    {
        public static string GetResourcePath()
        {
            return $"Static/{typeof(T).Name}";
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
            foreach (var item in UnityEngine.Resources.LoadAll<T>(GetResourcePath()))
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
            item.name = $"{typeof(T).Name}_{item.id:000000}";
            return item;
        }
    }
}