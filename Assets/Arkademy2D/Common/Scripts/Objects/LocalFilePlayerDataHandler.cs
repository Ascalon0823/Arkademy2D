using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy2D.Common.Objects
{
    [CreateAssetMenu(fileName = "New Player Data Handler", menuName = "Handler/PlayerData/LocalFile", order = 0)]
    public class LocalFilePlayerDataHandler : AbstractPlayerDataHandler
    {
        private const string PlayerDataExt = ".player";

        private string GetFolderPath()
        {
            return Application.persistentDataPath;
        }

        private string GetSaveFilePath(Guid playerGuid)
        {
            if (playerGuid == Guid.Empty)
            {
                return string.Empty;
            }

            return Path.Combine(GetFolderPath(), playerGuid.ToString(), PlayerDataExt);
        }

        private async Task<PlayerData> LoadPlayerDataFromFileAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                return JsonConvert.DeserializeObject<PlayerData>(
                    await File.ReadAllTextAsync(filePath, cancellationToken));
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to load player data at {filePath} {e.Message}");
                return null;
            }
        }

        public override async Task<IList<PlayerData>> GetAllPlayerDataAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Loading player data from file...", this);
            var results = new List<PlayerData>();
            var directory = new DirectoryInfo(GetFolderPath());
            foreach (var file in directory.GetFiles($"*{PlayerDataExt}"))
            {
                var playerData = await LoadPlayerDataFromFileAsync(file.FullName, cancellationToken);
                if (playerData is null) continue;
                results.Add(playerData);
            }

            return results;
        }


        public override async Task<PlayerData> LoadPlayerDataAsync(Guid playerGuid, CancellationToken cancellationToken)
        {
            var filePath = GetSaveFilePath(playerGuid);
            return await LoadPlayerDataFromFileAsync(filePath, cancellationToken);
        }

        public override async Task SavePlayerDataAsync(PlayerData playerData, CancellationToken cancellationToken)
        {
            if (playerData is null)
            {
                Debug.Log("Player data is empty");
                return;
            }

            var filePath = GetSaveFilePath(playerData.Guid);
            try
            {
                var json = JsonConvert.SerializeObject(playerData);
                await File.WriteAllTextAsync(filePath, json, cancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Unable to save player data to {filePath} {e.Message}");
            }
        }
    }
}