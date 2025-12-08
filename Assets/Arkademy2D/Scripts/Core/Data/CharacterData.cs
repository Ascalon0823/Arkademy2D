using System;
using Newtonsoft.Json;

namespace Arkademy2D.Core.Data
{
    public class CharacterData
    {
        [JsonProperty] public Guid Id { get; private set; } = Guid.NewGuid();
        [JsonProperty] public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
        public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;
        public string DisplayName { get; set; }
        public AcademicData AcademicData { get; set; } = new AcademicData();
    }
}