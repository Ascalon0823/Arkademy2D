using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Core.Store;

namespace Arkademy2D.Game.System
{
    public static class SaveSystem
    {
        private static IStore _store => new FileSystemStore();

        public static void SavePlayer(Core.Models.Player playerModel)
        {
            _store.SaveAsync(playerModel);
        }

        public static Core.Models.Player LoadPlayer(string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return LoadLastPlayedPlayer();
            }

            return _store.LoadAsync<Core.Models.Player>(key).Result;
        }

        public static List<Core.Models.Player> LoadAllPlayers()
        {
            return _store.LoadAllAsync<Core.Models.Player>().Result
                ?.OrderByDescending(x => x.LastUpdateDate)
                ?.ToList();
        }

        public static Core.Models.Player LoadLastPlayedPlayer()
        {
            var allPlayer = LoadAllPlayers();
            return allPlayer?.FirstOrDefault();
        }
    }
}