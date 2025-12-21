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
            var items = GameSystem.Character.CharacterModel.Items;
            var item = items.Count >= itemIdx ? items[itemIdx] : null;
            if (item == null || !ItemBase.Library.TryGetValue(item.ItemBaseId, out var itemBase) ||
                !itemBase.IsUsableItem)
            {
                return;
            }

            Debug.Log($"{name} uses {itemBase.displayName}");
            foreach (var usable in itemBase.usableBases)
            {
                if (!usable.HasEffect) continue;
                foreach (var effect in usable.usableEffects)
                {
                    effect.UseEffect(context);
                }
            }
        }
    }
}