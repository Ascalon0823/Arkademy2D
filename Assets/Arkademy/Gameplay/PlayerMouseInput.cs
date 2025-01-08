using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arkademy.Gameplay
{
    public class PlayerMouseInput : PlayerInputHandler
    {
        [SerializeField] private MouseButtonHandler primary = new MouseButtonHandler();
        [SerializeField] private MouseButtonHandler secondary = new MouseButtonHandler();

        protected override void HandleInput()
        {
            primary.onUIRaw = onUIRaw;
            secondary.onUIRaw = onUIRaw;
            primary.Update();
            secondary.Update();
            move = primary.currPosition - primary.startPosition;
            hold = secondary.pressed;
            holdPos = secondary.startPosition;
            holdDir = secondary.currPosition - secondary.startPosition;
            position = primary.position;
        }

        public void OnPosition(InputValue value)
        {
            primary.position = value.Get<Vector2>();
            secondary.position = value.Get<Vector2>();
        }

        public void OnPrimaryPress(InputValue value)
        {
            primary.pressed = value.isPressed;
        }

        public void OnSecondaryPress(InputValue value)
        {
            secondary.pressed = value.isPressed;
        }

        public void OnInteract(InputValue value)
        {
            interact = !onUIRaw;
        }


        [Serializable]
        private class MouseButtonHandler
        {
            public Vector2 position;
            public Vector2 startPosition;
            public Vector2 currPosition;
            public bool pressed;
            public bool wasPressed;
            public bool onUIRaw;
            public bool onUI;
            public void Update()
            {
                if (wasPressed && !pressed)
                {
                    onUI = false;
                    startPosition = Vector2.zero;
                    currPosition = Vector2.zero;
                    wasPressed = false;
                    return;
                }

                if (pressed && !wasPressed)
                {
                    wasPressed = pressed;
                    if (onUIRaw)
                    {
                        onUI = true;
                        return;
                    }
                    
                    startPosition = position;
                }

                if (!wasPressed || onUI) return;
                currPosition = position;
            }
        }
    }
}