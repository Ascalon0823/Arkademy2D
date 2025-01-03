using System;
using Arkademy.Common;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Player : MonoBehaviour
    {
        private static Player _localPlayer;
        public static Character Character => _localPlayer.character;
        public static Camera Camera => _localPlayer.followCamera.ppcam.cam;
        public Character character;
        public FollowCamera followCamera;
        public PlayerInput playerInput;

        public void Start()
        {
            _localPlayer = this;
            if (character) return;
            var characterData = Session.currCharacterRecord;
            characterData.LastPlayed = DateTime.UtcNow;
            character = Character.Create(characterData.character,0);
            followCamera.followTarget = character.transform;
        }

        private void Update()
        {
            if(!character || !playerInput) return;  
            character.Move(playerInput.moveDir);
            
        }
    }
}