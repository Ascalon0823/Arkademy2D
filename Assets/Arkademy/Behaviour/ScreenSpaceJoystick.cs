using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Arkademy.Behaviour
{
    public class ScreenSpaceJoystick : MonoBehaviour
    {
        [SerializeField] private CanvasScaler scaler;
        [SerializeField] private Vector2 touchBeginPos;
        [SerializeField] private Vector2 touchCurrPos;
        [SerializeField] private Vector2 pixelDelta;
        [SerializeField] private Vector2 normalizedDelta;
        public bool HasTouch => hasTouch;
        [SerializeField] private float maxMagnitude;
        [SerializeField] private RectTransform stickBase;
        [SerializeField] private RectTransform stickHandle;
        [SerializeField] private bool hasTouch;
        [SerializeField] private bool onUI;

        public Action<Vector2> OnDeltaUpdated;
        public Action<bool> OnFireUpdated;

        private void Start()
        {
            EnhancedTouchSupport.Enable();
            TouchSimulation.Enable();
            maxMagnitude = Mathf.FloorToInt(stickBase.sizeDelta.x / 2 - stickHandle.sizeDelta.x / 2);
        }

        private void OnDestroy()
        {
            TouchSimulation.Disable();
            EnhancedTouchSupport.Disable();
        }

        private void Update()
        {
            onUI = onUI || EventSystem.current.IsPointerOverGameObject();

            var rawHasTouch = Touch.activeTouches.Count > 0;
            if (!hasTouch && rawHasTouch && !onUI)
            {
                hasTouch = true;
                touchBeginPos = Touch.activeTouches[0].screenPosition;
                OnDeltaUpdated?.Invoke(normalizedDelta);
                OnFireUpdated?.Invoke(true);
            }

            if (!rawHasTouch)
            {
                if (hasTouch)
                {
                    OnFireUpdated?.Invoke(false);
                    OnDeltaUpdated?.Invoke(Vector2.zero);
                }

                onUI = false;
                hasTouch = false;
                normalizedDelta = Vector2.zero;
                pixelDelta = Vector2.zero;
            }

            stickBase.gameObject.SetActive(hasTouch);
            stickHandle.gameObject.SetActive(hasTouch);
            if (!hasTouch) return;

            OnFireUpdated?.Invoke(true);
            touchCurrPos = Touch.activeTouches[0].screenPosition;
            stickHandle.anchoredPosition = touchCurrPos / scaler.scaleFactor;
            var delta = touchCurrPos - touchBeginPos;
            var scaledMagnitude = maxMagnitude * scaler.scaleFactor;
            if (delta.magnitude > scaledMagnitude)
            {
                touchBeginPos = touchCurrPos - delta.normalized *
                    Mathf.Min(delta.magnitude, scaledMagnitude);
            }

            stickBase.anchoredPosition = touchBeginPos / scaler.scaleFactor;
            pixelDelta = touchCurrPos - touchBeginPos;
            normalizedDelta = pixelDelta / scaledMagnitude;
            OnDeltaUpdated?.Invoke(normalizedDelta);
        }
    }
}