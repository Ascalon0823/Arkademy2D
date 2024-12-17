using System;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class JoystickDisplay : MonoBehaviour
    {
        public PlayerInput playerInput;
        public RectTransform joyBase;
        public RectTransform joyTop;

        private void LateUpdate()
        {
            var show = playerInput.move.magnitude > 0 && playerInput.pressed;
            joyBase.gameObject.SetActive(show);
            joyTop.gameObject.SetActive(show);
            if (!show) return;
            joyBase.position = playerInput.startPosition;
            joyTop.position = playerInput.screenPosition;
        }
    }
}