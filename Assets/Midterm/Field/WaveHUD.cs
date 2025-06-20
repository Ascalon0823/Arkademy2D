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

        private void LateUpdate()
        {
            waveProgress.fillAmount = WaveManager.Instance.GetCurrWaveProgress();
            waveCountText.text = $"Wave {WaveManager.Instance.waveCount}";
        }
    }
}