using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.UI.Game
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private Fillbar healthBar;
        [SerializeField] private Fillbar xpBar;
        private void LateUpdate()
        {
            if (!PlayerBehaviour.PlayerChar) return;
            healthBar.fillAmount = PlayerBehaviour.PlayerChar.GetLifePercent();
            xpBar.fillAmount = PlayerBehaviour.PlayerChar.GetXpPercent();
        }
    }
}