using System;
using Arkademy2D.Core.Store;
using Newtonsoft.Json;

namespace Arkademy2D.Core.Models
{
    public class Player : IStoreKeyedData
    {
        [JsonProperty] public Guid Id { get; private set; } = Guid.NewGuid();
        [JsonProperty] public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
        public DateTime LastUpdateDate { get; set; }  = DateTime.UtcNow;
        public string DisplayName { get; set; }
        public string Key => Id.ToString();
    }
}