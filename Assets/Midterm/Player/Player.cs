using System;
using ArkademyStudio.PixelPerfect;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Midterm.Player
{
    public class Player : MonoBehaviour
    {

        public Vector2 playerMove;
        public Character.Character currCharacter;
        public PixelPerfectCamera currCamera;
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