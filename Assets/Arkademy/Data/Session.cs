using System.Linq;
using UnityEngine.Device;
using UnityEngine.SceneManagement;

namespace Arkademy.Data
{
    public static class Session
    {
        private static PlayerRecord _playerRecord;

        public static PlayerRecord currPlayerRecord
        {
            get
            {
                if (_playerRecord == null)
                {
                    if (Application.isEditor)
                    {
                        _playerRecord = PlayerRecord.LoadOrNew();
                        return _playerRecord;
                    }
                    SceneManager.LoadScene("Title");
                    return null;
                }

                return _playerRecord;
            }
            set => _playerRecord = value;
        }

        private static CharacterRecord _characterRecord;

        
        public static CharacterRecord currCharacterRecord
        {
            get
            {
                if (_characterRecord == null)
                {
                    var player = currPlayerRecord;
                    if (player.characterRecords.Count == 0)
                    {
                        SceneManager.LoadScene("CharacterCreation");
                        return null;
                    }

                    _characterRecord = player.characterRecords.OrderBy(x => x.LastPlayed).First();
                }
                return _characterRecord;
            }
            set => _characterRecord = value;
        }
        
        public static void Save()
        {
            currPlayerRecord.Save();
        }
    }
}