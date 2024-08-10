using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arkademy.UI.Game
{
    public class SpellCastJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private SpellCastMenu menu;

        [SerializeField] private bool pointerDown;
        [SerializeField] private Vector2 pressPosition;

        [SerializeField] private int pixelRange;
        [SerializeField] private float scaledRange;
        private void OnEnable()
        {
            pointerDown = false;
            scaledRange = pixelRange * GetComponentInParent<CanvasScaler>().scaleFactor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
            menu.BeginSpellUse(eventData.pressPosition);
            pressPosition = eventData.pressPosition;
        }

        private void Update()
        {
            if (!pointerDown) return;
            menu.UpdateSpellUse(Vector2.ClampMagnitude(
                EventSystem.current.currentInputModule.input.mousePosition - pressPosition, scaledRange) / scaledRange);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            menu.EndSpellUse(Vector2.ClampMagnitude(
                eventData.position - eventData.pressPosition, scaledRange) / scaledRange);
            pointerDown = false;
        }
    }
}