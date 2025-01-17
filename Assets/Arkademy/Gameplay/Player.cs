using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Common;
using Arkademy.Gameplay.Ability;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public enum AbilityBindingType
    {
        Tap,
        Hold,
        Swipe
    }

    [Serializable]
    public struct PlayerAbilityBinding
    {
        public AbilityBindingType binding;
        public AbilityBase ability;
    }

    public class Player : MonoBehaviour
    {
        private static Player _localPlayer;
        public static Player LocalPlayer => _localPlayer;
        public static Character Character => _localPlayer.character;
        public static Camera Camera => _localPlayer.followCamera.ppcam.cam;
        public Character character;
        [SerializeField] private FollowCamera cameraPrefab;
        
        public FollowCamera followCamera;
        public PlayerInput playerInput;
        public Interactable currentInteractableCandidate;
        public AbilityBase holdAbility;
        public AbilityBase tapAbility;
        public bool setupComplete;

        public void Start()
        {
            _localPlayer = this;
            if (setupComplete) return;
            Setup();
        }

        public void Setup()
        {
            var characterData = Session.currCharacterRecord;
            characterData.LastPlayed = DateTime.UtcNow;
            character = Character.Create(characterData.character, 0);
            var ai = character.GetOrAddComponent<CharacterAI>();
            ai.autoUseAbility = true;
            followCamera = Instantiate(cameraPrefab);
            followCamera.followTarget = character.transform;
        }

        public Vector2 GetRandomPosArrandCharacter(float distance)
        {
            return followCamera.GetRandomPosOutsideViewport(distance);
        }

        private void Update()
        {
            if (!character || !playerInput) return;
            character.Move(playerInput.moveDir);
            if (character.interactableDetector)
            {
                currentInteractableCandidate = character.interactableDetector.interactables
                    .OrderBy(x => Vector3.Distance(x.transform.position, character.transform.position))
                    .FirstOrDefault();
            }

            if (playerInput.interact && currentInteractableCandidate)
            {
                currentInteractableCandidate.OnInteractedBy(character);
            }

            if (tapAbility && playerInput.interact && !currentInteractableCandidate)
            {
                var e = new AbilityEventData
                {
                    Direction = playerInput.holdDir,
                    Position = playerInput.position,
                };
                if (tapAbility.CanUse(e)) tapAbility.Use(e);
            }

            if (holdAbility && playerInput.hold)
            {
                var e = new AbilityEventData
                {
                    Direction = playerInput.holdDir,
                    Position = playerInput.position,
                };
                if (holdAbility.CanUse(e)) holdAbility.Use(e);
            }

            if (holdAbility && !playerInput.hold && holdAbility.inUse)
            {
                holdAbility.Cancel();
            }
        }
    }
}