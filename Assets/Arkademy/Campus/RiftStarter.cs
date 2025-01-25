using Arkademy.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.Campus
{
    public class RiftStarter : Interactable
    {
        public override bool OnInteractedBy(Character character)
        {
            SceneManager.LoadScene("Rift");
            return true;
        }
    }
}