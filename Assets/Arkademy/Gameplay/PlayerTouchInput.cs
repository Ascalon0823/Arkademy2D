using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEditor.DeviceSimulation.TouchPhase;

namespace Arkademy.Gameplay
{
    public class PlayerTouchInput : PlayerInputHandler
    {
        [SerializeField] private float moveDeadZone = 10f;
        [SerializeField] private float analogRange = 100f;
        public Vector2 screenPosition;
        public Vector2 startPosition;
        [SerializeField] private bool onUI;
        public bool pressed;
        public Action<Vector2> onPressBegin;
        public Action<Vector2> onPressEnd;
        [SerializeField] private TouchState touch;

        protected override void HandleInput()
        {
            if (touch.isNoneEndedOrCanceled)
            {
                onPressEnd?.Invoke(touch.position);
                pressed = false;
                move = Vector2.zero;
                screenPosition = Vector2.zero;
                startPosition = Vector2.zero;
                hold = false;
                holdPos = Vector2.zero;
                holdDir = Vector2.zero;
                onUI = false;
                return;
            }

            if (!pressed)
            {
                if (onUIRaw) onUI = true;
                if (!onUI)
                {
                    startPosition = touch.startPosition;
                    onPressBegin?.Invoke(startPosition);
                }
            }

            if (onUI) return;
            pressed = true;
            screenPosition = touch.position;
            var delta = (touch.position - startPosition);
            delta = delta.magnitude < moveDeadZone ? Vector2.zero : delta;
            if (delta.magnitude > analogRange)
            {
                var clamped = Vector2.ClampMagnitude(delta, analogRange);
                startPosition += (delta - clamped);
                delta = screenPosition - startPosition;
            }

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began &&
                Time.realtimeSinceStartupAsDouble - touch.startTime >= holdThreshold)
            {
                hold = true;
                holdPos = touch.startPosition;
            }

            if (hold) holdDir = touch.position - holdPos;
            move = Vector2.ClampMagnitude(delta / analogRange, 1);
            position = screenPosition;
        }

        public void OnTouch(InputValue value)
        {
            touch = value.Get<TouchState>();
            if (!touch.isTap || onUI) return;
            interact = touch.isTap;
        }
    }
}