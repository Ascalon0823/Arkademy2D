using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Gameplay.Ability;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Data
{
    [Serializable]
    public class Item
    {
        public string baseName;
        public int stack;
    }

    [Serializable]
    public class ItemTag
    {
        public List<string> tags = new List<string>();
    }
    [Serializable]
    public class Equipment : Item
    {
        public List<Attribute.Modifier> additional = new List<Attribute.Modifier>();

        [JsonIgnore] public List<AbilityBase> providedAbilities = new List<AbilityBase>();
        public List<Attribute.Modifier> GetAllMods()
        {
            var result = new List<Attribute.Modifier>();
            var baseItem = ItemBase.GetItemBase(baseName);
            if (baseItem != null)
            {
                result.AddRange(baseItem.equipmentModifiers);
            }
            result.AddRange(additional);
            return result;
        }
    }

    [Serializable]
    public class EquipmentSlot
    {
        public int slot;
        public Equipment equipment;
    }

    [Serializable]
    public class ItemBase
    {
        private static Dictionary<string, ItemBase> _itemCache = new Dictionary<string, ItemBase>();
        public string baseName;
        public Sprite icon;
        public bool isEquipment;
        public List<Attribute.Modifier> equipmentModifiers = new List<Attribute.Modifier>();
        public int slot;
        public ItemTag tags;
        public List<string> ammunitionRequirements;
        public WeaponBaseAttack baseAttack;
        public AbilityPayload abilityPayload;
        public static ItemBase GetItemBase(string name)
        {
            if (!_itemCache.TryGetValue(name, out var cached))
            {
                cached = UnityEngine.Resources.Load<Scriptable.ItemBaseObject>(name).itemBase;
                _itemCache[name] = cached;
            }

            return cached;
        }

        public Item GetItem()
        {
            var item = new Item
            {
                stack = 1,
                baseName = baseName
            };
            if (isEquipment)
            {
                var equipment = (Equipment)item;
                equipment.additional = new List<Attribute.Modifier>();
                item = equipment;
            }
            return item;
        }
    }
}