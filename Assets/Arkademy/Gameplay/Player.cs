using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using Arkademy.Gameplay.Ability;
using Newtonsoft.Json;
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
        public CharacterRecord charaRecord;
        public FollowCamera followCamera;
        public PlayerInput playerInput;
        public Interactable currentInteractableCandidate;
        public AbilityBase holdAbility;
        public AbilityBase tapAbility;
        public AbilityBase swipeAbility;
        public bool autoUseTapAbility;
        public bool setupComplete;
        [TextArea] [SerializeField] private string currentPlayerData;

        public Vector3 characterStartPosition;

        public void Start()
        {
            _localPlayer = this;
            currentPlayerData = JsonConvert.SerializeObject(Session.currPlayerRecord);
            if (setupComplete) return;
            Setup();
        }

        public void Setup()
        {
            charaRecord = Session.currCharacterRecord;
            charaRecord.LastPlayed = DateTime.UtcNow;
            var race = Race.GetRace(charaRecord.character.raceName);
            character = Character.Create(race, charaRecord.character, 0);
            tapAbility = character.abilities.FirstOrDefault(x => x.abilityData.name == charaRecord.tapAbilityName);
            holdAbility = character.abilities.FirstOrDefault(x => x.abilityData.name == charaRecord.holdAbilityName);
            swipeAbility = character.abilities.FirstOrDefault(x => x.abilityData.name == charaRecord.swipeAbilityName);
            followCamera = Instantiate(cameraPrefab);
            followCamera.followTarget = character.transform;
            character.SetPosition(characterStartPosition);
        }

        public Vector2 GetRandomPosArrandCharacter(float distance)
        {
            return followCamera.GetRandomPosOutsideViewport(distance);
        }

        private void Update()
        {
            if (!character || !playerInput) return;
            character.wantToMove = playerInput.moveDir;
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
            
            var e = new AbilityEventData
            {
                PrimaryTarget = tapAbility?.GetPrimaryTarget(),
                Direction = playerInput.hold ? playerInput.holdDir : null,
                Position = (playerInput.hold || playerInput.interact) ? playerInput.position : null,
            };
            if (tapAbility  && ((autoUseTapAbility && tapAbility.CanReach(e)) || playerInput.interact) && !currentInteractableCandidate)
            {
                if (tapAbility.CanUse(e)) tapAbility.Use(e);
            }

            if (holdAbility && playerInput.hold)
            {
                if (holdAbility.CanUse(e)) holdAbility.Use(e);
            }

            if (holdAbility && (!playerInput.hold||!holdAbility.CanUse(e)) && holdAbility.InUse())
            {
                holdAbility.Use(e, true);
            }
        }
    }
}