using Newtonsoft.Json;
using UnityEngine;

namespace Midterm.Player
{
    public class SaveEngine
    {
        private static SaveEngine _instance;
        public static SaveEngine Instance = _instance ?? new SaveEngine();

        public virtual void Save(PlayerSaveData saveData)
        {
            var json = JsonConvert.SerializeObject(saveData);
            PlayerPrefs.SetString("player", json);
        }

        public virtual PlayerSaveData Load()
        {
            var pref = PlayerPrefs.GetString("player");
            return string.IsNullOrEmpty(pref) ? null : JsonConvert.DeserializeObject<PlayerSaveData>(pref);
        }
    }
}