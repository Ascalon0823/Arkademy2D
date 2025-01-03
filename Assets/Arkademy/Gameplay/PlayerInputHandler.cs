using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arkademy.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
      
        public string scheme;
        public Vector2 move;
        [SerializeField] protected bool onUIRaw;
        [SerializeField] protected UnityEngine.InputSystem.PlayerInput playerInput;

        protected virtual void Update()
        {
            onUIRaw = EventSystem.current.IsPointerOverGameObject();
            if (playerInput.currentControlScheme != scheme) return;
            HandleInput();
        }

        protected virtual void HandleInput()
        {
            
        }
    }
}