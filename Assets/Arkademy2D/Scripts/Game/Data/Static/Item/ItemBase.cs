using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Models = Arkademy2D.Core.Models;
namespace Arkademy2D.Game.Data.Static.Item
{
    public class ItemBase : StaticData<ItemBase>
    {
        // public const string ItemBaseResourcesPath = "Static/ItemBases"; 
        // public static Dictionary<string, ItemBase> Library
        // {
        //     get
        //     {
        //         if (_library == null)
        //         {
        //             _library = LoadAll();
        //         }
        //         return _library;
        //     }
        // }
        // private static Dictionary<string, ItemBase> _library;
        // private static Dictionary<string, ItemBase> LoadAll()
        // {
        //     var lib = new Dictionary<string, ItemBase>();
        //     foreach (var items in Resources.LoadAll<ItemBase>(ItemBaseResourcesPath))
        //     {
        //         lib.Add(items.Id, items);
        //     }
        //     return lib;
        // }
        //
        // private static void ReloadAll()
        // {
        //     _library = LoadAll();
        // }
        // [SerializeField] private string id;
        public string Id => id.ToString();
        public string displayName;
        public Sprite icon;

        // public static ItemBase Create()
        // {
        //     ReloadAll();
        //     var item = CreateInstance<ItemBase>();
        //     var id = Library.Count + 1;
        //     item.id = id.ToString();
        //     item.name = $"ItemBase_{id:000000}";
        //     return item;
        // }
    }
}