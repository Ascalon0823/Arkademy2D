using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.UI;

namespace Arkademy.Behaviour
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 look;
        public bool fire;

        [SerializeField] private ScreenSpaceJoystick joy;
        [SerializeField] private Player player;
        [SerializeField] private PlayerInput playerInput;

        public virtual void SetupForPlayer(Player newPlayer)
        {
            player = newPlayer;
            playerInput.camera = player.characterCamera.ppcam.cam;
            playerInput.uiInputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            joy.OnDeltaUpdated = null;
            joy.OnDeltaUpdated += OnTouchMove;
            HandleTouchInput();
        }


        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnFire(InputValue value)
        {
            if (EventSystem.current.IsPointerOverGameObject() && !fire) return;
            fire = value.isPressed;
        }

        public void OnLook(InputValue value)
        {
            look = value.Get<Vector2>();
        }

        private void OnTouchMove(Vector2 delta)
        {
            move = delta;
        }

        private void OnTouchFire(bool isFire)
        {
            fire = isFire;
        }

        private void HandleTouchInput()
        {
            var joyActive = Touchscreen.current != null;
            joy.gameObject.SetActive(joyActive);
        }

        protected virtual void Update()
        {
            HandleTouchInput();
        }
    }
}