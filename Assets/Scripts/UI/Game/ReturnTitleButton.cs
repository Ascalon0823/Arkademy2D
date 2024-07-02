using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.UI.Game
{
    public class ReturnTitleButton : MonoBehaviour
    {
        public void Return()
        {
            SceneManager.LoadScene("Title");
        }
    }
}
