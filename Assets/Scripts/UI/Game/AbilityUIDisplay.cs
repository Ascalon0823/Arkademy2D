using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.UI.Game
{
    public class AbilityUIDisplay : MonoBehaviour
    {
        public Ability ability;
        public Image icon;
        [SerializeField] private Image cooldown;
        [SerializeField] private TextMeshProUGUI level;

        private void LateUpdate()
        {
            if (!ability) return;
            cooldown.fillAmount = ability.remainingCooldown / ability.cooldown;
            level.text = ability.level.ToString();
        }
    }
}