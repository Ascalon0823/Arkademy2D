using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy2D.Core.Store
{
    public class FileSystemStore : IStore
    {
        private string GetFolderRoot<T>() where T : IStoreKeyedData
        {
            return Path.Combine(Application.persistentDataPath, typeof(T).Name);
        }

        public async Task<T> LoadAsync<T>(string key) where T : IStoreKeyedData
        {
            var path = Path.Combine(GetFolderRoot<T>(), key);
            try
            {
                var data = JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(path));
                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return default;
            }
        }

        public async Task SaveAsync<T>(T item) where T : IStoreKeyedData
        {
            var folder = GetFolderRoot<T>();
            Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, item.Key);
            await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(item));
        }
    }
}