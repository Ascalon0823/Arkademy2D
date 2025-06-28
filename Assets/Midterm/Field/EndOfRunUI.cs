using System;
using Midterm.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Midterm.Field
{
    public class EndOfRunUI : MonoBehaviour
    {
        public void Awake()
        {
            gameObject.SetActive(false);
        }

        public GameObject successUI;
        public GameObject failUI;
        public TextMeshProUGUI resultDetailText;

        public void Toggle()
        {
            if (isActiveAndEnabled) return;
            gameObject.SetActive(true);
            Time.timeScale = 0;
            successUI.SetActive(Player.Player.Local.currCharacter.life > 0);
            failUI.SetActive(Player.Player.Local.currCharacter.life <= 0);
            resultDetailText.text =
                $"You survived {WaveManager.Instance.waveCount} waves";
        }

        public void OnConfirm()
        {
            var player = Player.Player.Local;
            player.record.gold += WaveManager.Instance.waveCount;
            SaveEngine.Instance.Save(player.record);
            SceneManager.LoadScene("Midterm/Player/Title");
        }
    }
}