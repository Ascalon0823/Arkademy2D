using System.Linq;
using System.Threading.Tasks;
using Arkademy.Backend;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;

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
                    if (BackendService.Offline)
                    {
                        _playerRecord = PlayerRecord.LoadOrNew();
                        return _playerRecord;
                    }
                    SceneManager.LoadScene("UserAuth");
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

                    _characterRecord = player.characterRecords.OrderByDescending(x => x.LastPlayed).First();
                }
                return _characterRecord;
            }
            set => _characterRecord = value;
        }
        
        public static async Task Save()
        {
            currPlayerRecord.Save();
            if (BackendService.Offline) return;
            if (await BackendService.UpdatePlayer(currPlayerRecord) == null)
            {
                return;
            }
        }
    }
}