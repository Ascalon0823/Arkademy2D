using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arkademy2D.Common.Extensions;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy2D.Common.Objects
{
    [CreateAssetMenu(fileName = "New Player Data Handler", menuName = "Handler/PlayerData/PlayerPref", order = 0)]
    public class PlayerPrefPlayerDataHandler : AbstractPlayerDataHandler
    {
        private const string PlayerDataTableKey = "PlayerDatas";
        
        public override Task<IList<PlayerData>> GetAllPlayerDataAsync(CancellationToken cancellationToken)
        {
            Dictionary<Guid, PlayerData> playerDataTable = GetOrCreatePlayerDataTable();
            return Task.FromResult<IList<PlayerData>>(playerDataTable.Select(x => x.Value).ToList());
        }

        private Dictionary<Guid, PlayerData> GetOrCreatePlayerDataTable()
        {
            if (!PlayerPrefs.HasKey(PlayerDataTableKey))
            {
                Dictionary<Guid, PlayerData> playerDataTable = new Dictionary<Guid, PlayerData>();
                PlayerPrefs.SetString(PlayerDataTableKey, JsonConvert.SerializeObject(playerDataTable));
                PlayerPrefs.Save();
                return playerDataTable;
            }
            string playerDataTableJson = PlayerPrefs.GetString(PlayerDataTableKey);
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<Guid, PlayerData>>(playerDataTableJson);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                PlayerPrefs.DeleteKey(PlayerDataTableKey);
                return GetOrCreatePlayerDataTable();
            }
        }
        public override Task<PlayerData> LoadPlayerDataAsync(Guid playerGuid, CancellationToken cancellationToken)
        {
            try
            {
                Dictionary<Guid, PlayerData> playerDataTable = GetOrCreatePlayerDataTable();
                return playerDataTable.TryGetValue(playerGuid, out PlayerData playerData) ? Task.FromResult(playerData) : Task.FromResult<PlayerData>(null);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return Task.FromResult<PlayerData>(null);
            }
        }

        public override Task<PlayerData> SavePlayerDataAsync(PlayerData playerData, CancellationToken cancellationToken)
        {
            if (!playerData.Valid())
            {
                return Task.FromResult<PlayerData>(null);
            }
            Dictionary<Guid, PlayerData> playerDataTable = GetOrCreatePlayerDataTable();
            playerDataTable[playerData.Guid] = playerData;
            PlayerPrefs.SetString(PlayerDataTableKey, JsonConvert.SerializeObject(playerDataTable));
            PlayerPrefs.Save();
            return LoadPlayerDataAsync(playerData.Guid, cancellationToken);
        }
    }
}