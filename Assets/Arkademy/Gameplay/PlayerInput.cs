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

            move = inputHandler.move;
            moveDir = inputHandler.move.normalized;
        }
    }
}