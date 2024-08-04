using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.UI.Game
{
    public class Fillbar : MonoBehaviour
    {
        public Image fillImage;
        public float fillAmount;
        private void LateUpdate()
        {
            fillImage.fillAmount = fillAmount;
        }
    }
}