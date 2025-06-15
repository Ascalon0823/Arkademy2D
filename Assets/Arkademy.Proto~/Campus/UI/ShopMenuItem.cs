using System;
using Arkademy.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Campus.UI
{
    public class ShopMenuItem : MonoBehaviour
    {
        public ShopItem item;
        public ShopMenu shopMenu;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TextMeshProUGUI count;
        [SerializeField] private Image icon;
        [SerializeField] private Button remove;
        [SerializeField] private Button add;
        public int added;

        public void Setup(ShopItem item, ShopMenu shopMenu)
        {
            this.item = item;
            this.shopMenu = shopMenu;
            var baseItem = ItemBase.GetItemBase(item.item.baseName);
            icon.sprite = baseItem.icon;
            remove.onClick.AddListener(Remove);
            add.onClick.AddListener(Add);
        }

        private void Update()
        {
            text.text = $"{item.item.baseName}[{item.stock}] ${item.cost}";
            count.text = added.ToString();
            add.interactable = item.stock > 0 &&
                               Session.currCharacterRecord.character.gold >= item.cost + shopMenu.totalCost;
            remove.interactable = added > 0;
        }

        public void Add()
        {
            added++;
            item.stock--;
            shopMenu.totalCost+=item.cost;
        }

        public void Clear()
        {
            item.stock += added;
            shopMenu.totalCost -= item.cost * added;
            added = 0;
        }
        public void Remove()
        {
            added--;
            item.stock++;
            shopMenu.totalCost-=item.cost;
        }
    }
}