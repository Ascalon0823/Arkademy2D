using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy2D.Core.Store
{
    public class FileSystemStore : IStore
    {
        public static string RootPath => Application.persistentDataPath;
        private string GetFolderRootOfType<T>() where T : IStoreKeyedData
        {
            return Path.Combine(RootPath, typeof(T).Name);
        }

        public Task<T> LoadAsync<T>(string key) where T : IStoreKeyedData
        {
            var path = Path.Combine(GetFolderRootOfType<T>(), key);
            try
            {
                var data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                return Task.FromResult(data);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return Task.FromResult<T>(default);
            }
        }

        public Task SaveAsync<T>(T item) where T : IStoreKeyedData
        {
            var folder = GetFolderRootOfType<T>();
            Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, item.Key);
            File.WriteAllTextAsync(path, JsonConvert.SerializeObject(item));
            return Task.CompletedTask;
        }

        public Task<IList<T>> LoadAllAsync<T>() where T : IStoreKeyedData
        {
            var folder = GetFolderRootOfType<T>();
            Directory.CreateDirectory(folder);
            var result = new List<T>();
            foreach (var file in Directory.GetFiles(folder))
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var data = JsonConvert.DeserializeObject<T>(json);
                    result.Add(data);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            return Task.FromResult<IList<T>>(result);
        }
    }
}