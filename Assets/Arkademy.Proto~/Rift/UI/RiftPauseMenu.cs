using Arkademy.UI;
using UnityEngine.SceneManagement;

namespace Arkademy.Rift.UI
{
    public class RiftPauseMenu : PauseMenu
    {
        public void OnReturnToCampus()
        {
            SceneManager.LoadScene("Campus");
        }
    }
}