using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class LandingPage : MonoBehaviour
    {
        public void LandingPagePressed()
        {
            gameObject.SetActive(false);
            MainMenu.PlayerStarted();
        }
    }
}