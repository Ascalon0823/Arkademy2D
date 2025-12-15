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
        public static List<ItemBase> Modules
        {
            get
            {
                if (_modules == null)
                {
                    _modules = LoadAll();
                }
                return _modules;
            }
        }
        private static List<ItemBase> _modules;
        private static List<ItemBase> LoadAll()
        {
            return Resources.LoadAll<ItemBase>(ItemBaseResourcesPath).ToList();
        }

        [SerializeField] private string id;
        public string Id => id;
        public string displayName;
        public Sprite icon;
    }
}