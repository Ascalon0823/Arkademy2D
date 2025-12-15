using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Game.Data.Runtime;
using Arkademy2D.Core.Store;
using Arkademy2D.Core.Models;

namespace Arkademy2D.Game.System
{
    public static class SaveSystem
    {
        private static IStore _store => new FileSystemStore();

        public static void SavePlayer(PlayerData playerData)
        {
            _store.SaveAsync(playerData?.PlayerModel);
        }

        public static PlayerData LoadPlayer(string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return LoadLastPlayedPlayer();
            }

            var playerModel = _store.LoadAsync<Player>(key).Result;
            return playerModel == null ? null : new PlayerData { PlayerModel = playerModel };
        }

        public static List<PlayerData> LoadAllPlayers()
        {
            return _store.LoadAllAsync<Player>().Result
                ?.OrderByDescending(x => x.LastUpdateDate)
                ?.Select(x => new PlayerData { PlayerModel = x })
                ?.ToList();
        }

        public static PlayerData LoadLastPlayedPlayer()
        {
            var allPlayer = LoadAllPlayers();
            return allPlayer?.FirstOrDefault();
        }
    }
}