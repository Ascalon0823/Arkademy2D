using System;
using System.Linq;
using Arkademy.Data;
using Arkademy.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Attribute = Arkademy.Data.Attribute;

namespace Arkademy.Campus.UI
{
    public class InvestItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI attrDisplay;
        [SerializeField] private Button reduce;
        [SerializeField] private Button add;
        [SerializeField] private int originalInvestment;
        private AttrInvestment _investment;
        private AttributeProfile _profile;
        [SerializeField] private int nextCost;
        [SerializeField] private int prevCost;
        [SerializeField] private int level;
        private Attribute _attribute;
        public void Setup(AttributeProfile profile)
        {
            _profile = profile;
            _investment =
                Session.currCharacterRecord.character.attrInvestments.FirstOrDefault(x => x.type == profile.type);
            if (_investment == null)
            {
                _investment = new AttrInvestment
                {
                    type = profile.type,
                    xp = 0
                };
                Session.currCharacterRecord.character.attrInvestments.Add(_investment);
                Player.Character.Attributes.UpdateInvestment(_investment,profile);
            }

            _attribute = Player.Character.Attributes[_profile.type];
            originalInvestment = _investment.xp;
            reduce.onClick.AddListener(Reduce);
            add.onClick.AddListener(Invest);
        }

        private void Update()
        {
            nextCost = _profile.GetNextCost(_investment.xp);
            prevCost = _profile.GetPrevCost(_investment.xp);
            level = _profile.GetInvestLevel(_investment.xp);
            reduce.interactable = _investment.xp > originalInvestment;
            add.interactable = Session.currCharacterRecord.character.xp > nextCost;
            attrDisplay.text = $"{_profile.abbrev}[{level}]: {_attribute.Value()}";
        }

        public void ResetInvestment()
        {
            Session.currCharacterRecord.character.xp += _investment.xp - originalInvestment;
            _investment.xp = originalInvestment;
            Player.Character.Attributes.UpdateInvestment(_investment,_profile);
        }
        public void Invest()
        {
            _investment.xp += nextCost;
            Session.currCharacterRecord.character.xp -= nextCost;
            Player.Character.Attributes.UpdateInvestment(_investment,_profile);
        }

        public void Reduce()
        {
            _investment.xp -= prevCost;
            Session.currCharacterRecord.character.xp += prevCost;
            Player.Character.Attributes.UpdateInvestment(_investment,_profile);
        }
    }
}