using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Arkademy2D.Core.Models
{
    public class Character
    {
        [JsonProperty] public Guid Id { get; private set; } = Guid.NewGuid();
        [JsonProperty] public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
        public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;
        public string DisplayName { get; set; }
        public AcademicRecord AcademicRecord { get; set; } = new AcademicRecord();
        public int MaxHealth { get; set; }
        public float MoveSpeed { get; set; }
        public int MaxEnergy { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public Dictionary<int,int> Attributes { get; set; } = new Dictionary<int, int>();
    }
}