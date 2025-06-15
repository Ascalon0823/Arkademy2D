using System;
using Arkademy.Data;
using Arkademy.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Attribute = Arkademy.Data.Attribute;

namespace Arkademy.UI
{
    public class AttrDisplayItem : MonoBehaviour
    {
        
        [SerializeField] private Attribute attribute;
        [SerializeField] private AttributeProfile profile;
        [SerializeField] private AttrInvestment investment;
        [SerializeField] private TextMeshProUGUI attrText;
        
        [SerializeField] private TextMeshProUGUI investmentText;
        [SerializeField] private Button reduce;
        [SerializeField] private Button add;

        [SerializeField] private StatsPage statsPage;

        public void Setup(StatsPage page,Attribute attr, AttributeProfile attrProfile, AttrInvestment attrInvestment)
        {
            statsPage = page;
            attribute = attr;
            profile = attrProfile;
            investment = attrInvestment;
            reduce.onClick.RemoveAllListeners();
            reduce.onClick.AddListener(()=>statsPage.Decrease(profile, investment));
            add.onClick.RemoveAllListeners();
            add.onClick.AddListener(()=>statsPage.Increase(profile, investment));
            
        }

        private void Update()
        {
            attrText.text = $"{profile.abbrev}: {Player.Character.Attributes.Get(profile.type)}";
            investmentText.text = $"{statsPage.GetTotalLevel(profile, investment)}";
            reduce.interactable = statsPage.CanDecrease(profile);
            add.interactable = statsPage.CanIncrease(profile,investment);
        }
    }
}