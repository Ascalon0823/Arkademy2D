using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arkademy.Gameplay
{
    public class JoystickDisplay : MonoBehaviour
    {
        public PlayerInput input;
        public RectTransform joyBase;
        public RectTransform joyTop;

        private void LateUpdate()
        {
            var show = input.move.magnitude > 0;
            joyBase.gameObject.SetActive(show);
            joyTop.gameObject.SetActive(show);
            if (!show) return;
            joyBase.position = input.position - input.move;
            joyTop.position = input.position;
        }
    }
}