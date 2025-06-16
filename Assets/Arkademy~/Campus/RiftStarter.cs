using Arkademy.Campus.UI;
using Arkademy.Gameplay;
using Arkademy.Rift;
using UnityEngine;
using UnityEngine.SceneManagement;
using Character = Arkademy.Gameplay.Character;

namespace Arkademy.Campus
{
    public class RiftStarter : Interactable
    {
        public override bool OnInteractedBy(Character character)
        {
            RiftStartMenu.Show();
            return true;
        }
    }
}