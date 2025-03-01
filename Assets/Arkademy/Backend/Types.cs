using System;
using Newtonsoft.Json.Linq;
using UnityEngine.Serialization;

namespace Arkademy.Backend
{
    [Serializable]
    public class User
    {
        public string id;
        public string username;
        public PlayerRecord playerRecord;
    }

    [Serializable]
    public class PlayerRecord
    {
        public DateTime CreationTime;
        public DateTime LastPlayedTime;
        public CharacterRecord[] characters;
    }

    [Serializable]
    public class CharacterRecord
    {
        public string displayName;
        public DateTime CreationTime;
        public DateTime LastPlayedTime;
        public JToken Data;
    }
}