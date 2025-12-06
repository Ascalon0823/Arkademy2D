using System.Linq;
using Arkademy2D.Core.Data;

namespace Arkademy2D.Core.System
{
    public static class GameSystem
    {
        public static PlayerData PlayerData
        {
            get
            {
                if (_playerData == null)
                {
                    _playerData = SaveSystem.LoadPlayer() ?? new PlayerData();
                }

                return _playerData;
            }
            set { _playerData = value; }
        }

        private static PlayerData _playerData;

        public static CharacterData CharacterData
        {
            get
            {
                if (_characterData == null)
                {
                    var characters = PlayerData.Characters;
                    if (characters.Count == 0)
                    {
                        characters.Add(new CharacterData());
                    }
                    _characterData = characters.OrderByDescending(x => x.LastUpdateDate).First();
                }

                return _characterData;
            }
            set { _characterData = value; }
        }

        private static CharacterData _characterData;
    }
}