using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using TMPro;
using UnityEngine;

namespace Arkademy.Campus.UI
{
    public class ShopMenu : MonoBehaviour
    {
        private static ShopMenu _instance;

        [SerializeField] private int originalXp;
        [SerializeField] private ShopMenuItem itemPrefab;
        [SerializeField] private List<ShopMenuItem> spawnedItem = new();
        [SerializeField] private RectTransform content;
        [SerializeField] private TextMeshProUGUI goldText;
        public int totalCost;

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            Toggle(false,null);
        }

        public static void Show(Shop shop)
        {
            _instance.Toggle(true, shop);
        }

        public void Toggle(bool active, Shop shop)
        {
            gameObject.SetActive(active);
            if (active)
            {
                Setup(shop);
            }
        }

        private void Update()
        {
            goldText.text = $"{Session.currCharacterRecord.character.gold - totalCost}";
        }

        public void Setup(Shop shop)
        {
            foreach (var item in spawnedItem)
            {
                Destroy(item.gameObject);
            }

            spawnedItem.Clear();
            totalCost = 0;
            foreach (var shopItem in shop.items)
            {
                var item = Instantiate(itemPrefab, content);
                item.Setup(shopItem,this);
                spawnedItem.Add(item);
            }
        }

        

        public async void Cancel()
        {
            foreach (var item in spawnedItem)
            {
                item.Clear();
            }

            await Session.Save();
            Toggle(false,null);
        }

        public async void Confirm()
        {
            foreach (var item in spawnedItem)
            {
                for (var i = 0; i < item.added; i++)
                {
                    Session.currCharacterRecord.character.AddToInventory(item.item.item);
                }
            }
            await Session.Save();
            Toggle(false,null);
        }
    }
}