using System;
using System.Threading.Tasks;

namespace Arkademy2D.Core.Store
{
    public interface IStore
    {
        Task<T> LoadAsync<T>(string key) where T : IStoreKeyedData;
        Task SaveAsync<T>(T item) where T : IStoreKeyedData;
    }
}