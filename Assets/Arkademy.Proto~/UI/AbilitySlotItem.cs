using System;
using Arkademy.Data;
using Arkademy.Gameplay;
using Arkademy.Gameplay.Ability;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.UI
{
    public class AbilitySlotItem : MonoBehaviour
    {
        public Ability.AbilitySlotType slotType;
        public string abilityName;

        public Image icon;
        public TextMeshProUGUI abilityText;
        public AbilityPage page;
        public void Init()
        {
            var characterRecord = Session.currCharacterRecord;
            if (slotType == Ability.AbilitySlotType.Tap)
            {
                abilityName = characterRecord.tapAbilityName;
            }

            if (slotType == Ability.AbilitySlotType.Hold)
            {
                abilityName = characterRecord.holdAbilityName;
            }
            UpdateUI();
        }

        public void SetAbility(string newAbility)
        {
            abilityName = newAbility;
            var charaRecord = Session.currCharacterRecord;
            if (slotType == Ability.AbilitySlotType.Tap)
            {
                charaRecord.tapAbilityName = abilityName;
            }

            if (slotType == Ability.AbilitySlotType.Hold)
            {
                charaRecord.holdAbilityName = abilityName;
            }
            Player.LocalPlayer.UpdateAbilityBindings();
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (string.IsNullOrEmpty(abilityName))
            {
                icon.enabled = false;
                abilityText.text = $"{slotType} ability";
                return;
            }
            icon.enabled = true;
            var ability = Data.Ability.GetAbility(abilityName);
            abilityText.text = ability.displayName;
            icon.sprite = ability.icon;
        }

        public void OnClick()
        {
            page.BeginSelection(this);
        }
    }
}