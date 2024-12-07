using System;
using Arkademy.Common;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Player : MonoBehaviour
    {
        public Character character;
        public FollowCamera followCamera;

        public void Start()
        {
            if (character) return;
            var characterData = Session.currCharacterRecord;
            characterData.LastPlayed = DateTime.UtcNow;
            character = Character.Create(characterData.character);
            followCamera.followTarget = character.transform;
        }
    }
}