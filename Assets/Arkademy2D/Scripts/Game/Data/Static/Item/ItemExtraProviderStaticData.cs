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
        
    }
}