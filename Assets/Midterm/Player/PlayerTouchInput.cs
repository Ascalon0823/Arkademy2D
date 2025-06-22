using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Midterm.Player
{
    public class PlayerTouchInput : MonoBehaviour
    {
        public TouchState touch;
        public bool onUI;
        public Player player;
        public PlayerInput playerInput;
        public Transform joyBase;
        public Transform joyTop;

        public void OnTouch(InputValue value)
        {
            touch = value.Get<TouchState>();
        }

        private void Update()
        {
            if (playerInput.currentControlScheme.ToLower() != "touch")
            {
                joyBase.gameObject.SetActive(false);
                return;
            }

            if (touch.isNoneEndedOrCanceled)
            {
                joyBase.gameObject.SetActive(false);
                onUI = false;
                player.playerMove = Vector2.zero;
                return;
            }

            if (touch.phase == TouchPhase.Began && player.rawOnUI)
            {
                joyBase.gameObject.SetActive(false);
                onUI = true;
                return;
            }

            joyBase.gameObject.SetActive(false);
            if (onUI) return;
            joyBase.gameObject.SetActive(true);
            var diff = touch.position - touch.startPosition;
            var dir = diff.normalized;
            var scaledPixelDist = diff.magnitude / player.currCamera.pixelScale;
            var wordDist = scaledPixelDist / 16 * (player.currCamera.camRect.width/4f);
            player.playerPointAt = player.currCamera.cam.WorldToScreenPoint(dir*  wordDist + player.currCharacter.body.position);
            player.playerMove = dir;
            joyBase.transform.position = player.currCamera.GetWorldPos(touch.startPosition);
            joyTop.transform.position = player.currCamera.GetWorldPos(touch.position);
        }
    }
}