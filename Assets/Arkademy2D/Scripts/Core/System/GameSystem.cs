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
                    _playerData = SaveSystem.LoadPlayer()??new PlayerData();
                }
                return _playerData;
            }
            set
            {
                _playerData = value;
            }
        }
        private static PlayerData _playerData;
    }
}