using System.Linq;
using Arkademy2D.Core.Data;
using Arkademy2D.Core.Data.Runtime;
using Arkademy2D.Core.Models;
using UnityEngine;

namespace Arkademy2D.Core.System
{
    public static class GameSystem
    {
        public static PlayerData Player
        {
            get
            {
                if (Application.isEditor && _playerData == null)
                {
                    _playerData = SaveSystem.LoadLastPlayedPlayer() ?? new PlayerData { PlayerModel = new Player() };
                }

                return _playerData;
            }
            set => _playerData = value;
        }

        private static PlayerData _playerData;

        public static CharacterData Character
        {
            get
            {
                if (Application.isEditor && _characterData == null)
                {
                    if (Player.PlayerModel.Characters.Count == 0)
                    {
                        Player.PlayerModel.Characters.Add(new Character());
                    }

                    var lastModel = Player.PlayerModel.Characters
                        .OrderByDescending(x => x.LastUpdateDate)
                        .FirstOrDefault();
                    _characterData = new CharacterData { CharacterModel = lastModel };
                }

                return _characterData;
            }
        }

        private static CharacterData _characterData;

        public static void SaveGame()
        {
            SaveSystem.SavePlayer(Player);
        }
    }
}