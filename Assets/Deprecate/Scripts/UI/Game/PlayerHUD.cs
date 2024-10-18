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
            if (!Player.Chara) return;
            healthBar.fillAmount = Player.Chara.GetLifePercent();
            xpBar.fillAmount = Player.Chara.GetXpPercent();
        }
    }
}