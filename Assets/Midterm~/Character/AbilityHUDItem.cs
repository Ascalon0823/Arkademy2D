using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Midterm.Character
{
    public class AbilityHUDItem : MonoBehaviour
    {
        public Ability targetAbility;
        public Image icon;
        public Image cooldown;
        public TextMeshProUGUI lvText;
        
        private void LateUpdate()
        {
            icon.sprite = targetAbility.icon;
            cooldown.fillAmount = targetAbility.GetCooldownPercentage();
            lvText.text = (targetAbility.currLevel + 1).ToString();
        }
    }
}