using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Midterm.Character
{
    public class PlayerHUD : MonoBehaviour
    {
        public Image lifeFill;
        public Image levelFill;
        public TextMeshProUGUI lvText;
        private void LateUpdate()
        {
            var level = Player.Player.Local.currCharacter.GetComponent<Level>();
            
            lvText.text = $"Lv {level.currLevel+1}";
            levelFill.fillAmount = level.GetXPPercentage();
            lifeFill.fillAmount = Player.Player.Local.currCharacter.life*1f/Player.Player.Local.currCharacter.maxLife;
        }
    }
}