using System;
using System.Collections.Generic;
using ArkademyStudio.PixelPerfect;
using Midterm.Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Midterm.Player
{
    [Serializable]
    public class PlayerSaveData
    {
        public int playCount;
        public int gold;
        public List<string> unlockedCharacters;
    }

    public class Player : MonoBehaviour
    {
        public static Player Local;
        public static string SelectedCharacterInternalName;
        public PlayerSaveData record;
        public Vector2 playerMove;

        public Vector2 playerPointAt;
        public bool playerCasting;
        public bool playerPreparing;
        public Character.Character currCharacter;
        public PixelPerfectCamera currCamera;

        public bool rawOnUI;
        public bool onUI;

        private void Awake()
        {
            Local = this;
            var loaded = SaveEngine.Instance.Load();
            if (loaded == null)
            {
                loaded = new PlayerSaveData();
                SaveEngine.Instance.Save(loaded);
            }

            record = loaded;
            Debug.Log(SelectedCharacterInternalName);
            var selectedPlayableCharacter = PlayableCharacterData.Get(SelectedCharacterInternalName);
            currCharacter.SetupAs(selectedPlayableCharacter);
        }

        public void OnMove(InputValue value)
        {
            playerMove = value.Get<Vector2>();
        }

        private void Update()
        {
            rawOnUI = EventSystem.current.IsPointerOverGameObject();
            currCharacter.moveDir = playerMove;
            currCharacter.preparing = playerPreparing;
            currCharacter.pointAt = currCamera.GetWorldPos(playerPointAt);
            //currCharacter.casting = playerCasting;
        }

        public void OnPointAt(InputValue value)
        {
            playerPointAt = value.Get<Vector2>();
        }

        public void OnCast(InputValue value)
        {
            playerCasting = value.isPressed && (!rawOnUI || playerCasting);
        }

        public void OnPrepare(InputValue value)
        {
            playerPreparing = value.isPressed && (!rawOnUI || playerPreparing);
        }

        
    }
}