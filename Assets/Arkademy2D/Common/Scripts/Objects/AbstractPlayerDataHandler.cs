using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arkademy2D.Common.Interfaces;
using UnityEngine;

namespace Arkademy2D.Common.Objects
{
    public abstract class AbstractPlayerDataHandler : ScriptableObject, IPlayerDataHandler
    {
        #if UNITY_EDITOR
        [SerializeField] private List<PlayerData> loadedData;

        [ContextMenu("Load Data")]
        private void LoadData()
        {
            loadedData =  GetAllPlayerDataAsync(CancellationToken.None).Result.ToList();
        }
        #endif
        public abstract Task<IList<PlayerData>> GetAllPlayerDataAsync(CancellationToken cancellationToken);

        public abstract Task<PlayerData> LoadPlayerDataAsync(Guid playerGuid, CancellationToken cancellationToken);

        public abstract Task<PlayerData>
            SavePlayerDataAsync(PlayerData playerData, CancellationToken cancellationToken);
    }
}