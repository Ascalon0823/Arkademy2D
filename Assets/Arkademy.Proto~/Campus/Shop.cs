using System;
using System.Collections.Generic;
using Arkademy.Campus.UI;
using Arkademy.Data;
using Arkademy.Gameplay;
using UnityEngine;
using Character = Arkademy.Gameplay.Character;

namespace Arkademy.Campus
{

    [Serializable]
    public class ShopItem
    {
        public Item item;
        public int stock;
        public int cost;
    }
    
    public class Shop : Interactable
    {
        public List<ShopItem> items = new List<ShopItem>();
        public override bool OnInteractedBy(Character character)
        {
            ShopMenu.Show(this);
            return true;
        }
    }
}