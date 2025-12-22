using System;
using System.Collections.Generic;
using Arkademy2D.Game.Behaviours;
using Arkademy2D.Game.Data.Static.Item;
using Arkademy2D.Game.System;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Game.UI.HUD
{
    public class HotBar : MonoBehaviour
    {
        public GameObject selectionIndicator;
        public List<GameObject> itemsHolders = new List<GameObject>();
        private void Start()
        {
            var items = GameSystem.CharacterModel.Items;
            for(var i =0;i<itemsHolders.Count; i++)
            {
                if (items.Count > i)
                {
                    var item = items[i];
                    var id = item.ItemBaseId;
                    if (ItemBase.Library.TryGetValue(id, out var itemBase))
                    {
                        itemsHolders[i].GetComponentInChildren<Image>().sprite = itemBase.icon;
                    }
                }
                var i1 = i;
                itemsHolders[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    Player.Local.selectedHotbarIdx = i1;
                });
            }
            
        }

        private void LateUpdate()
        {
            selectionIndicator.transform.position = itemsHolders[Player.Local.selectedHotbarIdx].transform.position;
        }
    }
}