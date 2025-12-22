using System;
using Arkademy2D.Game.System;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours
{
    public class Player : MonoBehaviour
    {
        public Core.Models.Player Model;
        public Character character;
        public static Player Local;
        private void Awake()
        {
            Local = this;
        }

        private void Start()
        {
            Setup(GameSystem.PlayerModel);
        }

        public void Setup(Core.Models.Player player)
        {
            Model = player;
            character.SetupUseCharacterData(GameSystem.CharacterModel);
        }
    }
}