using System.Collections.Generic;
using Arkademy2D.Game.Data.Static.Usable;
using UnityEngine;

namespace Arkademy2D.Game.Data.Static.Item
{
    public class ItemBase : StaticData<ItemBase>
    {
        public string Id => id.ToString();
        public string displayName;
        public Sprite icon;
        public List<UsableBase> usableBases;
        public bool IsUsableItem => usableBases?.Count > 0;
    }
}