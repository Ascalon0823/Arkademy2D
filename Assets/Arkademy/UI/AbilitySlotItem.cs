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
        public string slotType;
        public string abilityName;

        public Image icon;
        public TextMeshProUGUI abilityText;

        public void Init()
        {
            var characterRecord = Session.currCharacterRecord;
            if (slotType == "tap")
            {
                abilityName = characterRecord.tapAbilityName;
            }

            if (slotType == "hold")
            {
                abilityName = characterRecord.holdAbilityName;
            }
            UpdateUI();
        }

        public void SetAbility(string newAbility)
        {
            abilityName = newAbility;
            var charaRecord = Session.currCharacterRecord;
            if (slotType == "tap")
            {
                charaRecord.tapAbilityName = abilityName;
            }

            if (slotType == "hold")
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
    }
}