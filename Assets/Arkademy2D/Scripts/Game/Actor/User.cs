using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Game.Data.Static.Item;
using Arkademy2D.Game.System;
using UnityEngine;

namespace Arkademy2D.Game.Actor
{
    public class User : MonoBehaviour
    {
        public void UseItem(int itemIdx, UseContext context)
        {
            var items = GameSystem.Character.ItemData;
            var item = items.Count >= itemIdx ? items[itemIdx] : null;
            foreach (var usableData in item.usableData)
            {
                if (usableData.Use(context))
                {
                    Debug.Log($"{name} uses {item.itemBase.displayName}");
                }
            }
        }
    }
}