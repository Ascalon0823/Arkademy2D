using Arkademy2D.Core.Models;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Item
{
    public abstract class ItemExtraProviderStaticData : StaticData
    {
        public abstract ItemExtra GetDefaultItemExtra();
    }
    public abstract class ItemExtraProviderStaticData<T> : ItemExtraProviderStaticData
        where T : ItemExtraProviderStaticData<T>
    {
        public override string GetItemAssetName()
        {
            var actualDisplayName = string.IsNullOrWhiteSpace(displayName) ? "Unknown" : displayName; 
            return $"{typeof(T).Name}_{id:000000}_{actualDisplayName}";
        }
    }
}