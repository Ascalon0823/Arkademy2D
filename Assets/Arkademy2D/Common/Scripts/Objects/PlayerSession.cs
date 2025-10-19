using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arkademy2D.Common.Extensions;
using Arkademy2D.Common.Interfaces;
using UnityEngine;

namespace Arkademy2D.Common.Objects
{
    [CreateAssetMenu(fileName = "PlayerSession", menuName = "Sessions/Player", order = 0)]
    public class PlayerSession : ScriptableObject
    {
        public static PlayerSession Curr
        {
            get
            {
                if (!_curr)
                {
                    var existing = Resources.LoadAll<PlayerSession>("Configs").FirstOrDefault();
                    if (!existing) throw new Exception($"Instance {nameof(PlayerSession)} does not exist");
                    _curr = existing;
                }
                return _curr;
            }
        }

        private static PlayerSession _curr;
        public AbstractPlayerDataHandler playerDataHandler;
        public PlayerData localPlayerData;

        public async Task<PlayerData> GetPlayerDataAsync(CancellationToken token)
        {
            if (localPlayerData.Valid()) return localPlayerData;
            var playerList = await playerDataHandler.GetAllPlayerDataAsync(token);
            var first = playerList.FirstOrDefault();
            if (!first.Valid()) return null;
            localPlayerData = first;
            return localPlayerData;
        }

        public async Task<PlayerData> CreatePlayerDataAsync(string displayName, CancellationToken token)
        {
             var newPlayerData = new PlayerData
            {
                Guid = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,
                LastUpdateTime = DateTime.UtcNow,
                displayName =displayName,
            };
            
            return await playerDataHandler.SavePlayerDataAsync(newPlayerData, token);
        }
    }
}