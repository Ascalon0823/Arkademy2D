using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arkademy2D.Common.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy2D.Common.Objects
{
    public abstract class AbstractPlayerDataHandler : ScriptableObject, IPlayerDataHandler
    {
#if UNITY_EDITOR
        [SerializeField, TextArea(10,1000)] private string loadedData;

        [ContextMenu("Load Data")]
        private void LoadData()
        {
            loadedData = JsonConvert.SerializeObject(GetAllPlayerDataAsync(CancellationToken.None).Result.ToList(), Formatting.Indented);
        }
#endif
        public abstract Task<IList<PlayerData>> GetAllPlayerDataAsync(CancellationToken cancellationToken);

        public abstract Task<PlayerData> LoadPlayerDataAsync(Guid playerGuid, CancellationToken cancellationToken);

        public abstract Task<PlayerData>
            SavePlayerDataAsync(PlayerData playerData, CancellationToken cancellationToken);
    }
}