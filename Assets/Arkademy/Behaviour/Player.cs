using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

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

        public void Setup(Game.Session.PlayerSetup setup)
        {
            playerRecord = setup.playerRecord;
            controllingCharacter = Instantiate(playerCharacterPrefab);
            characterCamera = Instantiate(playerCameraPrefab);
            controllingCharacter.Setup(setup.characterRecord.characterData,1);
            controllingCharacter.tag = "Player";
            characterCamera.followTarget = controllingCharacter.transform;
        }

        private void Update()
        {
            if (Application.isEditor)
            {
                desireUse = Input.GetMouseButton(0);
                desireMovDir = Vector2.zero;
                desireMovDir += Input.GetKey(KeyCode.W) ? Vector2.up : Vector2.zero;
                desireMovDir += Input.GetKey(KeyCode.S) ? Vector2.down : Vector2.zero;
                desireMovDir += Input.GetKey(KeyCode.A) ? Vector2.left : Vector2.zero;
                desireMovDir += Input.GetKey(KeyCode.D) ? Vector2.right : Vector2.zero;
            }
            controllingCharacter.MoveDir(desireMovDir);
            if (desireUse) controllingCharacter.Use();
        }
    }
}