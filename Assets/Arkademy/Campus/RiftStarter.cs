using Arkademy.Data;
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
            RiftController.RiftSetup = Session.currCharacterRecord.clearedDifficulty + 1;
            SceneManager.LoadScene("Rift");
            return true;
        }
    }
}