using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Arkademy
{
    public class Player : MonoBehaviour
    {
        public static int? UsingCharacterDBIdx;
        public static int? UsingStageDBIdx;
        public static bool Paused => Curr.paused;
        public static Player Curr;
        public static PixelPerfectCamera Cam;
        public static CharacterBehaviour Chara;
        public UnityEvent<CharacterBehaviour> onPlayerCharLevelUp;

        [SerializeField] private int editorCharaDBIdx;

        public int pauseCount;
        public bool paused => pauseCount > 0;

        private void Awake()
        {
            if (Curr && Curr != this)
            {
                Destroy(gameObject);
                return;
            }

            Curr = this;
            if (Application.isEditor && !UsingCharacterDBIdx.HasValue)
            {
                UsingCharacterDBIdx = editorCharaDBIdx;
            }
        }

        public CharacterBehaviour playerCharacter;
        public ScreenSpaceJoystick joystick;
        public FollowCamera playerCamera;
        public CharacterBehaviour playerCharacterPrefab;

        private void Start()
        {
            if (!playerCharacter)
            {
                var charaData = Database.GetDatabase().playableCharacterData[UsingCharacterDBIdx ?? 0];
                playerCharacter = Instantiate(playerCharacterPrefab);
                playerCharacter.animator.runtimeAnimatorController = charaData.animatorController;
                playerCharacter.gameObject.SetLayerRecursive(LayerMask.NameToLayer("Player"));
                playerCharacter.charaData = charaData;
                playerCharacter.life = charaData.life;
            }

            playerCharacter.onLevelUp.AddListener(chara => { onPlayerCharLevelUp?.Invoke(chara); });

            playerCamera.followTarget = playerCharacter.transform;
            Chara = playerCharacter;
            Cam = playerCamera.ppcam;
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