using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Models = Arkademy2D.Core.Models;
namespace Arkademy2D.Game.Data.Static.Item
{
    [CreateAssetMenu(fileName = "ItemBase", menuName = ItemBaseResourcesPath)]
    public class ItemBase : ScriptableObject
    {
        private const string ItemBaseResourcesPath = "Static/ItemBases"; 
        public static Dictionary<string, ItemBase> Library
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
        private static Dictionary<string, ItemBase> _library;
        private static Dictionary<string, ItemBase> LoadAll()
        {
            var lib = new Dictionary<string, ItemBase>();
            foreach (var items in Resources.LoadAll<ItemBase>(ItemBaseResourcesPath))
            {
                lib.Add(items.Id, items);
            }
            return lib;
        }
        [SerializeField] private string id;
        public string Id => id;
        public string displayName;
        public Sprite icon;
    }
}