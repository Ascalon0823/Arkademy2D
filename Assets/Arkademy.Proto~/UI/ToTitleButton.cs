using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.UI
{
    public class ToTitleButton : MonoBehaviour
    {
        public void ToTitle()
        {
            SceneManager.LoadScene("Title");
        }
    }
}