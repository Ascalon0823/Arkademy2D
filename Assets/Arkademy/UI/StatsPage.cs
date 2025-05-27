using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using Arkademy.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.UI
{
    public class StatsPage : Page
    {
        [SerializeField] private TextMeshProUGUI xpDisplay;
        [SerializeField] private RectTransform investmentRoot;

        [Serializable]
        private class StatsChange
        {
            public int totalCost;
            public Dictionary<AttributeProfile, int> InvestmentChange = new Dictionary<AttributeProfile, int>();
        }
        [SerializeField] private StatsChange statsChange;
        [SerializeField] private AttrDisplayItem itemPrefab;
        [SerializeField] private List<AttrDisplayItem> spawnedItems = new List<AttrDisplayItem>();
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
         public override void OnShow()
        {
            base.OnShow();
            var character = Session.currCharacterRecord;
            xpDisplay.text = character.character.xp.ToString();
           Reset();
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(Reset);
        }

        public bool CanIncrease(AttributeProfile profile, AttrInvestment investment)
        {
            var currTotalInvestment = (investment?.xp ?? 0) + statsChange.InvestmentChange[profile];
            var nextLevelCost = profile.GetNextCost(currTotalInvestment);
            return Session.currCharacterRecord.character.xp - statsChange.totalCost >= nextLevelCost;
        }
        
        public bool CanDecrease(AttributeProfile profile)
        {
            return statsChange.InvestmentChange[profile] > 0;
        }

        public void Increase(AttributeProfile profile, AttrInvestment investment)
        {
            var diff = profile.GetNextCost((investment?.xp ?? 0) + statsChange.InvestmentChange[profile]);
            statsChange.InvestmentChange[profile] += diff;
            statsChange.totalCost += diff;
        }

        public void Decrease(AttributeProfile profile, AttrInvestment investment)
        {
            var diff = profile.GetPrevCost((investment?.xp ?? 0) + statsChange.InvestmentChange[profile]);
            statsChange.InvestmentChange[profile] -= diff;
            statsChange.totalCost -= diff;
        }

        public int GetTotalLevel(AttributeProfile profile, AttrInvestment investment)
        {
            return profile.GetInvestLevel((investment?.xp ?? 0) + statsChange.InvestmentChange[profile]);
        }
        private void Update()
        {
            var minusString = statsChange.totalCost == 0 ? "" : $"- {statsChange.totalCost}";
            xpDisplay.text = $"{Session.currCharacterRecord.character.xp} {minusString}";
            confirmButton.interactable = statsChange.totalCost != 0;
            cancelButton.interactable = statsChange.totalCost != 0;
        }

        private void OnConfirm()
        {
            Session.currCharacterRecord.character.xp -= statsChange.totalCost;
            foreach (var pair in statsChange.InvestmentChange)
            {
                var investment =
                    Session.currCharacterRecord.character.attrInvestments.FirstOrDefault(x => x.type == pair.Key.type);
                if (investment == null)
                {
                    investment = new AttrInvestment
                    {
                        type = pair.Key.type,
                    };
                    Session.currCharacterRecord.character.attrInvestments.Add(investment);
                }
                investment.xp += pair.Value;
                Player.Character.Attributes.UpdateInvestment(investment,pair.Key);
            }
            Reset();
        }

        private void Reset()
        {
            statsChange = new StatsChange();
            foreach (var item in spawnedItems)
            {
                Destroy(item.gameObject);
            }
            spawnedItems.Clear();
            foreach (var profile in Session.currCharacterRecord.character.GetAllAttributeProfiles())
            {
                if (!profile.canInvest) continue;
                statsChange.InvestmentChange[profile] = 0;
                var item = Instantiate(itemPrefab, investmentRoot);
                item.Setup(this, Player.Character.Attributes[profile.type],profile,
                    Session.currCharacterRecord.character.attrInvestments.FirstOrDefault(x=>x.type == profile.type));
                spawnedItems.Add(item);
            }
        }
    }
}