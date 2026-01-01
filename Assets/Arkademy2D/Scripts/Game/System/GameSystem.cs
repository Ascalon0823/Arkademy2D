using System.Linq;
using Arkademy2D.Core.Models;
using UnityEngine;
namespace Arkademy2D.Game.System
{
    public static class GameSystem
    {
        public static Player PlayerModel
        {
            get
            {
                if (Application.isEditor && _playerModel == null)
                {
                    _playerModel = SaveSystem.LoadLastPlayedPlayer() ?? new Player();
                }

                return _playerModel;
            }
            set => _playerModel = value;
        }

        private static Player _playerModel;

        public static Character CharacterModel
        {
            get
            {
                if (Application.isEditor && _characterData == null)
                {
                    if (PlayerModel.Characters.Count == 0)
                    {
                        PlayerModel.Characters.Add(new Character());
                    }

                    var lastModel = PlayerModel.Characters
                        .OrderByDescending(x => x.LastUpdateDate)
                        .FirstOrDefault();
                    _characterData = lastModel;
                }

                return _characterData;
            }
        }

        private static Character _characterData;

        public static void SaveGame()
        {
            SaveSystem.SavePlayer(PlayerModel);
        }
    }
}