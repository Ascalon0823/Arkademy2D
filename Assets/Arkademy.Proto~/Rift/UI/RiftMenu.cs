using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Rift.UI
{
    public class RiftMenu : MonoBehaviour
    {
        public Image riftProgressDisplay;
        public Image timeDisplay;
        public RiftController riftController;
        private void Update()
        {
            if (!riftController) return;
            riftProgressDisplay.fillAmount = riftController.progress / 1000f;
            timeDisplay.fillAmount = riftController.passedTime / 300f;
        }
    }
}