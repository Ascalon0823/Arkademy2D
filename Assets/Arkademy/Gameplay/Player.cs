using System;
using System.Linq;
using Arkademy.Common;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Player : MonoBehaviour
    {
        private static Player _localPlayer;
        public static Player LocalPlayer => _localPlayer;
        public static Character Character => _localPlayer.character;
        public static Camera Camera => _localPlayer.followCamera.ppcam.cam;
        public Character character;
        public FollowCamera followCamera;
        public PlayerInput playerInput;
        public Interactable currentInteractableCandidate;

        public void Start()
        {
            _localPlayer = this;
            if (character) return;
            var characterData = Session.currCharacterRecord;
            characterData.LastPlayed = DateTime.UtcNow;
            character = Character.Create(characterData.character, 0);
            followCamera.followTarget = character.transform;
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
        }
    }
}