using Arkademy2D.Game.Data.Static.Item;
using Arkademy2D.Game.System;
using UnityEngine;

namespace Arkademy2D.Game.Actor
{
    public class User : MonoBehaviour
    {
        public void UseItem(int itemIdx)
        {
            var items = GameSystem.Character.CharacterModel.Items;
            var item = items.Count>=itemIdx ? items[itemIdx] : null;
            if (item != null && ItemBase.Library.TryGetValue(item.ItemBaseId, out var itemBase))
            {
                Debug.Log($"{name} uses {itemBase.displayName}");
            }
        }
    }
}