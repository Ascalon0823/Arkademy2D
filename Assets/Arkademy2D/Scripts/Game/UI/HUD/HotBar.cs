using System.Collections.Generic;
using Arkademy2D.Game.Data.Static.Item;
using Arkademy2D.Game.System;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Game.UI.HUD
{
    public class HotBar : MonoBehaviour
    {
        public List<GameObject> itemsHolders = new List<GameObject>();
        private void Start()
        {
            var items = GameSystem.Character.CharacterModel.Items;
            for(var i =0;i<items.Count; i++)
            {
                var item = items[i];
                var id = item.ItemBaseId;
                if (ItemBase.Library.TryGetValue(id, out var itemBase))
                {
                    itemsHolders[i].GetComponentInChildren<Image>().sprite = itemBase.icon;
                }
            }
        }
    }
}