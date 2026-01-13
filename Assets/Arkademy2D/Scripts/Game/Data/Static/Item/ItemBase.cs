using System.Collections.Generic;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Item
{
    public class ItemBase : StaticData<ItemBase>
    {
        public Sprite icon;
      
        public List<AttributeConfig> attributesConfigs = new List<AttributeConfig>();

        public Core.Models.Item GetDefaultItemModel()
        {
            var item = new Core.Models.Item
            {
                ItemBaseId = id
            };

            return item;
        }
    }
}