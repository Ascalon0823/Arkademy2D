using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arkademy.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
      
        public string scheme;
        public Vector2 move;
        public bool interact;
        public Vector2 interactPos;
        [SerializeField] protected bool onUIRaw;
        [SerializeField] protected UnityEngine.InputSystem.PlayerInput playerInput;

        protected virtual void Update()
        {
            onUIRaw = EventSystem.current.IsPointerOverGameObject();
            if (playerInput.currentControlScheme != scheme) return;
            HandleInput();
        }

        protected virtual void LateUpdate()
        {
            if (interact) interact = false;
        }

        protected virtual void HandleInput()
        {
            
        }
    }
}