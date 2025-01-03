using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arkademy.Gameplay
{
    public class JoystickDisplay : MonoBehaviour
    {
        public PlayerTouchInput touchInput;
        public RectTransform joyBase;
        public RectTransform joyTop;

        private void LateUpdate()
        {
            var show = touchInput.move.magnitude > 0 && touchInput.pressed;
            joyBase.gameObject.SetActive(show);
            joyTop.gameObject.SetActive(show);
            if (!show) return;
            joyBase.position = touchInput.startPosition;
            joyTop.position = touchInput.screenPosition;
        }
    }
}