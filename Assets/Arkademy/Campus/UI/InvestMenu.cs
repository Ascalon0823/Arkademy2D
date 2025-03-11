using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using TMPro;
using UnityEngine;

namespace Arkademy.Campus.UI
{
    public class InvestMenu : MonoBehaviour
    {
        private static InvestMenu _instance;

        [SerializeField] private int originalXp; 
        [SerializeField] private InvestItem itemPrefab;
        [SerializeField] private List<InvestItem> spawnedItem = new();
        [SerializeField] private RectTransform content;
        [SerializeField] private TextMeshProUGUI xpChangeText;

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            Toggle(false);
        }

        public static void Show()
        {
            _instance.Toggle(true);
        }

        public void Toggle(bool active)
        {
            gameObject.SetActive(active);
            if (active)
            {
                Setup();
            }
        }

        public void Setup()
        {
            foreach (var item in spawnedItem)
            {
                Destroy(item.gameObject);
            }
            spawnedItem.Clear();
            originalXp = Session.currCharacterRecord.character.xp;
            var race = Race.GetRace(Session.currCharacterRecord.character.raceName);
            var investables = race.attributes.Where(x => x.canInvest).ToList();
            foreach (var investable in investables)
            {
                var item = Instantiate(itemPrefab, content);
                spawnedItem.Add(item);
                item.Setup(investable);
            }
        }

        public void Update()
        {
            xpChangeText.text= $"XP: {originalXp} > {Session.currCharacterRecord.character.xp}";
        }

        public async void Cancel()
        {
            foreach (var item in spawnedItem)
            {
                item.ResetInvestment();
            }

            await Session.Save();
            Toggle(false);
        }

        public async void Confirm()
        {
            await Session.Save();
            Toggle(false);
        }
    }
}