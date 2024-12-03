using System;
using Newtonsoft.Json;
using UnityEngine;
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
        public TouchState touch;
        [TextArea] public string touchData;

        public void OnTouch(InputValue value)
        {
            touch = value.Get<TouchState>();
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
            fire = touch.isTap;
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
            if (touch.isTap)
            {
                onFire?.Invoke(touch.position);
            }
        }
    }
}