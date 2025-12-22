using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Core.Models;
using Arkademy2D.Game.Data.Static.Item;
using UnityEngine;

namespace Arkademy2D.Game.Data.Runtime
{
    [Serializable]
    public class CharacterData
    {
        public Character CharacterModel;
        public List<ItemData> ItemData{
            get
            {
                if (_itemData == null)
                {
                    _itemData = GetItemData();
                }

                return _itemData;
            }
        }
        
        [SerializeField]private List<ItemData> _itemData;

        private List<ItemData> GetItemData()
        {
            if (CharacterModel?.Items?.Count == 0)
            {
                return new List<ItemData>();
            }

            return CharacterModel.Items.Select(x =>
            {
                var baseItem = ItemBase.Library.GetValueOrDefault(x.ItemBaseId);
                var usables = baseItem.usableBases.Select(y => new UsableData(y));
                return new ItemData
                {
                    itemBase = baseItem,
                    usableData = usables.ToList()
                };
            }).ToList();
        }

        public void Update(float deltaTime)
        {
            foreach (var itemData in ItemData)
            {
                itemData.Update(deltaTime);
            }
        }
    }
}