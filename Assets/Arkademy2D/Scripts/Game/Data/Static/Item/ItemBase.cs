using System.Collections.Generic;
using Arkademy2D.Core.Models;
using Arkademy2D.Game.Data.Static.Usable;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Item
{
    public interface IItemExtraProvider
    {
        ItemExtra GetDefaultItemExtra();
    }
    public class ItemBase : StaticData<ItemBase>
    {
        public string Id => id.ToString();
        public string displayName;
        public Sprite icon;
        public List<UsableBase> usableBases;
        public bool IsUsableItem => usableBases?.Count > 0;
        public List<ItemExtraProviderStaticData> ItemExtraProviders = new List<ItemExtraProviderStaticData>();

        public Core.Models.Item GetDefaultItemModel()
        {
            var item = new Core.Models.Item
            {
                ItemBaseId = id
            };
            foreach (var provider in ItemExtraProviders)
            {
                var extra = provider.GetDefaultItemExtra();
                item.Extras.AddExtra(extra.GetType().Name,provider.GetDefaultItemExtra());
            }

            return item;
        }
    }
}