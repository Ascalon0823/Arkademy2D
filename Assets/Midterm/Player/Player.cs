using System;
using ArkademyStudio.PixelPerfect;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Midterm.Player
{
    public class Player : MonoBehaviour
    {
        public static Player Local;
        public Vector2 playerMove;
        public Character.Character currCharacter;
        public PixelPerfectCamera currCamera;

        private void Awake()
        {
            Local = this;
        }

        public void OnMove(InputValue value)
        {
            playerMove = value.Get<Vector2>();
        }

        private void Update()
        {
            currCharacter.moveDir = playerMove;
        }
    }
}