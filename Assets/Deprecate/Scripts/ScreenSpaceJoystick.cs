using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arkademy
{
    public class ScreenSpaceJoystick : MonoBehaviour
    {
        [SerializeField] private TouchInputHandler handler;
        [SerializeField] private CanvasScaler scaler;
        public Vector2 touchBeginPos;
        public Vector2 touchCurrPos;
        public Vector2 pixelDelta;
        public Vector2 normalizedDelta;
        [SerializeField] private float maxMagnitude;
        [SerializeField] private RectTransform stickBase;
        [SerializeField] private RectTransform stickHandle;
        [SerializeField] private bool hasTouch;
        [SerializeField] private bool onUI;

        private void Update()
        {
            onUI = onUI || EventSystem.current.IsPointerOverGameObject();
            if (!hasTouch && handler.hasTouch && !onUI)
            {
                hasTouch = true;
                touchBeginPos = handler.currPos;
            }

            if (!handler.hasTouch)
            {
                onUI = false;
                hasTouch = false;
                normalizedDelta = Vector2.zero;
                pixelDelta = Vector2.zero;
            }

            stickBase.gameObject.SetActive(hasTouch);
            stickHandle.gameObject.SetActive(hasTouch);
            if (!hasTouch) return;

            touchCurrPos = handler.currPos;
            stickHandle.anchoredPosition = touchCurrPos / scaler.scaleFactor;
            var delta = touchCurrPos - touchBeginPos;
            if (delta.magnitude > maxMagnitude)
            {
                touchBeginPos = touchCurrPos - delta.normalized *
                    Mathf.Min(delta.magnitude, maxMagnitude);
            }

            stickBase.anchoredPosition = touchBeginPos / scaler.scaleFactor;
            pixelDelta = touchCurrPos - touchBeginPos;
            normalizedDelta = pixelDelta / maxMagnitude;
        }
    }
}