using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Midterm.Field
{
    public class WaveHUD : MonoBehaviour
    {
        public Image waveProgress;
        public TextMeshProUGUI waveCountText;

        public GameObject bossHpDisplay;
        public TextMeshProUGUI bossNameText;
        public Image bossHpFill;
        private void LateUpdate()
        {
            waveProgress.fillAmount = WaveManager.Instance.GetCurrWaveProgress();
            waveCountText.text = $"Wave {WaveManager.Instance.waveCount +1}";
            bossHpDisplay.SetActive(WaveManager.Instance.currentBoss);
            if (WaveManager.Instance.currentBoss)
            {
                bossNameText.text = $"{WaveManager.Instance.currentBoss.displayName}";
                var bossChar = WaveManager.Instance.currentBoss.GetComponent<Character.Character>();
                bossHpFill.fillAmount =bossChar.life*1f/bossChar.maxLife;
            }
            
        }
    }
}