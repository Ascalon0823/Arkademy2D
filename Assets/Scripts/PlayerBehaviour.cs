using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public static int UsingCharacterDBIdx;
        public static PlayerBehaviour Player;

        public int pauseCount;

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
                var charaPrefab = Database.GetDatabase().playableCharacterData[UsingCharacterDBIdx].characterPrefab;
                playerCharacter = Instantiate(charaPrefab);
            }

            playerCamera.followTarget = playerCharacter.transform;
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