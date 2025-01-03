using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Arkademy.Gameplay
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private UnityEngine.InputSystem.PlayerInput playerInput;
        [SerializeField] private float moveDeadZone = 10f;
        [SerializeField] private float analogRange = 100f;
        public Vector2 screenPosition;
        public Vector2 startPosition;
        public Vector2 moveDir;
        public Vector2 move;
        public bool pressed;
        public Action<Vector2> onPressBegin;
        public Action<Vector2> onPressEnd;
        public Action<Vector2> onFire;
        public bool fire;
        public bool onUI;
        [SerializeField] private bool onUIRaw;
        public TouchState touch;

        private void Update()
        {
            onUIRaw = EventSystem.current.IsPointerOverGameObject();
            HandleTouch();
        }

        public void HandleTouch()
        {
            if (playerInput.currentControlScheme != "Touch") return;
            if (fire)
            {
                onFire?.Invoke(touch.position);
                fire = false;
            }
            if (touch.isNoneEndedOrCanceled)
            {
                onPressEnd?.Invoke(touch.position);
                pressed = false;
                move = Vector2.zero;
                moveDir = Vector2.zero;
                screenPosition = Vector2.zero;
                startPosition = Vector2.zero;
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
            delta = delta.magnitude<moveDeadZone ? Vector2.zero:delta;
            if (delta.magnitude > analogRange)
            {
                var clamped = Vector2.ClampMagnitude(delta, analogRange);
                startPosition += (delta - clamped);
                delta = screenPosition - startPosition;
            }
            moveDir = delta.normalized;
            move = Vector2.ClampMagnitude(delta / analogRange, 1);
            
        }
        public void OnTouch(InputValue value)
        {
            touch = value.Get<TouchState>();
            if(touch.isTap)
                fire = touch.isTap;
        }

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
            moveDir = move.normalized;
        }
    }
}