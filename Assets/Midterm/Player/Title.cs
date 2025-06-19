using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Midterm.Player
{
    public class Title : MonoBehaviour
    {
        public PlayerSaveData saveData;
        public PlayableCharacterSelection selection;
        private void Awake()
        {
            saveData = SaveEngine.Instance.Load();
            if (saveData == null)
            {
                saveData = new PlayerSaveData();
                SaveEngine.Instance.Save(saveData);
            }
            selection.Toggle(false);
        }

        public void StartGame()
        {
            selection.Toggle(true);
        }

        [ContextMenu("Save")]
        public void Save()
        {
            if (saveData != null)
            {
                SaveEngine.Instance.Save(saveData);
            }
        }
        
    }
}