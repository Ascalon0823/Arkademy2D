using Arkademy2D.Game.Data.Static;
using Arkademy2D.Game.System;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours
{
    public class Player : MonoBehaviour
    {
        public Core.Models.Player Model;
        public Character character;
        public Character characterPrefab;
        public static Player Local;
        public int selectedHotbarIdx;
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
            if (!character)
            {
                character = Instantiate(characterPrefab);
            }
            character.model = GameSystem.CharacterModel;
            character.Setup();
        }
    }
}