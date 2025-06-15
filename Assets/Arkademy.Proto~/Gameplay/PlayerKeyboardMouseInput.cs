using UnityEngine;
using UnityEngine.InputSystem;

namespace Arkademy.Gameplay
{
    public class PlayerKeyboardMouseInput : PlayerInputHandler
    {
        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }
    }
}