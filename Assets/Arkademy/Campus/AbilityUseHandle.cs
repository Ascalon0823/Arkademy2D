using System;
using Arkademy.Gameplay.Ability;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arkademy.Campus
{
    public class AbilityUseHandle : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public bool casting;
        public Vector2 startPosition;
        public Vector2 currPosition;
        public Vector2 dragDirection;

        public void OnBeginDrag(PointerEventData eventData)
        {
            casting = true;
            startPosition = eventData.position;
            dragDirection = Vector2.zero;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            casting = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            currPosition = eventData.position;
            dragDirection = currPosition - startPosition;
        }
    }
}