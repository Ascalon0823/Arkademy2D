using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public static int UsingCharacterDBIdx;
        public static PlayerBehaviour Player;
        public static PixelPerfectCamera PlayerCam;
        public static CharacterBehaviour PlayerChar;

        public int pauseCount;
        public bool paused => pauseCount > 0;

        private void Awake()
        {
            if (Player && Player != this)
            {
                Destroy(gameObject);
                return;
            }

            Player = this;
        }

        public CharacterBehaviour playerCharacter;
        public ScreenSpaceJoystick joystick;
        public FollowCamera playerCamera;

        private void Start()
        {
            if (!playerCharacter)
            {
                var charaData = Database.GetDatabase().playableCharacterData[UsingCharacterDBIdx];
                var charaPrefab = charaData.characterPrefab;
                playerCharacter = Instantiate(charaPrefab);
                playerCharacter.gameObject.SetLayerRecursive(LayerMask.NameToLayer("Player"));
                playerCharacter.charaData = charaData;
            }
            playerCamera.followTarget = playerCharacter.transform;
            PlayerChar = playerCharacter;
            PlayerCam = playerCamera.ppcam;
        }

        private void Update()
        {
            Time.timeScale = pauseCount > 0 ? 0 : 1;
            if (playerCharacter && joystick)
            {
                playerCharacter.wantToMove = joystick.normalizedDelta.normalized;
            }
        }
    }
}