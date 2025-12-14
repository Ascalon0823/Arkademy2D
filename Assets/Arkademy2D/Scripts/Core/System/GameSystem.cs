using System.Linq;
using Arkademy2D.Core.Data;
using Arkademy2D.Core.Models;
using UnityEngine;

namespace Arkademy2D.Core.System
{
    public static class GameSystem
    {
        public static PlayerData PlayerData
        {
            get
            {
                if (Application.isEditor && _playerData == null)
                {
                    _playerData = SaveSystem.LoadLastPlayedPlayer() ?? new PlayerData{PlayerModel = new Player()};
                }
                return _playerData;
            }
            set => _playerData = value;
        }
        private static PlayerData _playerData;
        public static CharacterData CharacterData {
            get
            {
                if (Application.isEditor && _characterData == null)
                {
                    if (PlayerData.PlayerModel.Characters.Count == 0)
                    {
                        
                        PlayerData.PlayerModel.Characters.Add(new Character());
                    }

                    var lastModel = PlayerData.PlayerModel.Characters
                        .OrderByDescending(x => x.LastUpdateDate)
                        .FirstOrDefault();
                    _characterData = new CharacterData{CharacterModel =  lastModel};
                }
                return _characterData;
            } 
        }
        private static CharacterData _characterData;

        public static void SaveGame()
        {
            SaveSystem.SavePlayer(PlayerData);
        }
    }
}