using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arkademy
{
    public class TouchInputHandler : MonoBehaviour
    {
        public bool hasTouch;
        public Vector2 currPos;
        public void OnPosition(InputAction.CallbackContext ctx)
        {
            currPos = ctx.ReadValue<Vector2>();
        }

        public void OnTouch(InputAction.CallbackContext ctx)
        {
            hasTouch = ctx.ReadValueAsButton();
        }
    }

}
