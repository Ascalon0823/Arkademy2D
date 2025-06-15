using System;
using ArkademyStudio.PixelPerfect;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arkademy
{
    public class Player : MonoBehaviour
    {
        public Character currCharacter;
        public PixelPerfectCamera currCamera;

        [SerializeField] private Transform crosshair;
        [SerializeField] private bool tryUse;
        [SerializeField] private Vector2 pointAtScreen;
        [SerializeField] private Vector2 pointAtWorld;

        public void OnMove(InputValue value)
        {
            currCharacter.moveDir = value.Get<Vector2>().normalized;
        }

        public void OnPointAt(InputValue value)
        {
            pointAtScreen = value.Get<Vector2>();
        }

        public void OnUse(InputValue value)
        {
            tryUse = value.isPressed;
        }

        private void Update()
        {
            if (tryUse)
            {
                currCharacter.Use();
            }

            pointAtWorld = currCamera.GetWorldPos(pointAtScreen);
            currCharacter.pointAt = pointAtWorld;
            crosshair.transform.position = pointAtWorld;
        }
    }
}