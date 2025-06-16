using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private UnityEngine.InputSystem.PlayerInput playerInput;
        public Vector2 moveDir;
        public Vector2 move;
        public bool interact;
        public Vector2 position;
        public bool hold;
        public Vector2 holdPos;
        public Vector2 holdDir;
        public List<PlayerInputHandler> playerInputHandlers = new();

        private Dictionary<string, PlayerInputHandler> playerInputHandlersDictionary = new();

        private void Start()
        {
            foreach (var playerInputHandler in playerInputHandlers)
            {
                playerInputHandlersDictionary[playerInputHandler.scheme] = playerInputHandler;
            }
        }

        private void Update()
        {
            if (!playerInputHandlersDictionary.TryGetValue(playerInput.currentControlScheme, out var inputHandler))
            {
                return;
            }

            position = inputHandler.position;
            move = inputHandler.move;
            moveDir = inputHandler.move.normalized;
            interact = inputHandler.interact;
            hold = inputHandler.hold;
            holdPos = inputHandler.holdPos;
            holdDir = inputHandler.holdDir;
        }
    }
}