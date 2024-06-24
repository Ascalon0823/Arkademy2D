using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.UI.Title
{
    public class TileMenu : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void DeleteSaveData()
        {
            
        }
    }
}
