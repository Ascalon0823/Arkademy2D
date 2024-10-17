using System;
using System.Collections.Generic;
using System.IO;
using Arkademy.Behaviour.UI;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Arkademy.Behaviour
{
    public class Player : MonoBehaviour
    {
        public PlayerRecord playerRecord;
        public bool local;
        public Character controllingCharacter;
        public FollowCamera characterCamera;

        public Vector2 desireMovDir;
        public bool desireUse;
        [SerializeField] private Character playerCharacterPrefab;
        [SerializeField] private FollowCamera playerCameraPrefab;

        public PlayerInputHandler playerInput;
        public PlayerMenu playerMenu;
        public void Setup(Game.Session.PlayerSetup setup, bool isLocal)
        {
            playerRecord = setup.playerRecord;
            controllingCharacter = Instantiate(playerCharacterPrefab);
            characterCamera = Instantiate(playerCameraPrefab);
            controllingCharacter.Setup(setup.characterRecord.characterData,1);
            controllingCharacter.tag = "Player";
            characterCamera.followTarget = controllingCharacter.transform;
            local = isLocal;
            if (!local)
            {
                Destroy(playerInput.gameObject);
                Destroy(playerMenu.gameObject);
                return;
            }
            playerInput.SetupForPlayer(this);
        }

        private void Update()
        {
            if (!local) return;
            desireMovDir = playerInput.move;
            desireUse = playerInput.fire;
            controllingCharacter.MoveDir(desireMovDir);
            if (desireUse) controllingCharacter.Use();
        }

      
    }
}