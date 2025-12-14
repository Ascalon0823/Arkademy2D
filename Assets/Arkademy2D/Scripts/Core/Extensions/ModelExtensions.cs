using Arkademy2D.Core.Store;

namespace Arkademy2D.Core.Extensions
{
    public static class ModelExtensions
    {
        public static bool Valid<T>(this T item) where T : IStoreKeyedData
        {
            return item != null && !string.IsNullOrEmpty(item.Key);
        }
    }
}