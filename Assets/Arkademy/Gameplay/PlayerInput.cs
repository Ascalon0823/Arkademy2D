using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Arkademy.Gameplay
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float moveDeadZone = 10f;
        [SerializeField] private float analogRange = 100f;
        public Vector2 screenPosition;
        public Vector2 moveDir;
        public Vector2 moveAnalog;
        public Vector2 moveRaw;
        public bool pressed;
        public Action<Vector2> onPressBegin;
        public Action<Vector2> onPressEnd;
        public Action<Vector2> onFire;
        public bool fire;
        public bool onUI;
        [SerializeField] private bool onUIRaw;
        public TouchState touch;
        [TextArea] public string touchData;

        private void Update()
        {
            onUIRaw = EventSystem.current.IsPointerOverGameObject();
            HandleTouch();
        }

        public void HandleTouch()
        {
            if (touch.isNoneEndedOrCanceled)
            {
                onUI = false;
            }
            else
            {
                if (touch.phase == TouchPhase.Began && onUIRaw)
                    onUI = true;
            }

            if (onUI) return;
            touchData = JsonConvert.SerializeObject(touch, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            screenPosition = touch.position;
            var delta = (touch.position - touch.startPosition);
            delta = delta.magnitude<moveDeadZone ? Vector2.zero:delta;
            moveDir = delta.normalized;
            moveAnalog = Vector2.ClampMagnitude(delta / analogRange, 1);
            moveRaw = delta;
            
            if (touch.phase == TouchPhase.Began)
            {
                onPressBegin?.Invoke(touch.position);
                pressed = true;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                onPressEnd?.Invoke(touch.position);
                pressed = false;
            }
            if (fire)
            {
                onFire?.Invoke(touch.position);
                fire = false;
            }
        }
        public void OnTouch(InputValue value)
        {
            touch = value.Get<TouchState>();
            if(touch.isTap)
                fire = touch.isTap;
        }
    }
}