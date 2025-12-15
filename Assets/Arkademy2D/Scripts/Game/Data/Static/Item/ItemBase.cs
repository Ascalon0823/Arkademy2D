using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Item
{
    public class ItemBase : StaticData<ItemBase>
    {
        public string Id => id.ToString();
        public string displayName;
        public Sprite icon;
    }
}