using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arkademy.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
      
        public string scheme;
        public Vector2 move;
        public bool confineMove;
        public int confineDistance;
        public bool interact;
        public Vector2 position;
        public bool hold;
        public Vector2 holdPos;
        public Vector2 holdDir;
        [SerializeField] protected bool onUIRaw;
        [SerializeField] protected UnityEngine.InputSystem.PlayerInput playerInput;
        [SerializeField] protected float holdThreshold = 0.25f;
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