using Arkademy.Campus.UI;
using Arkademy.Gameplay;
using UnityEngine;

namespace Arkademy.Campus
{
    public class InvestStarter : Interactable
    {
        public override bool OnInteractedBy(Character character)
        {
            InvestMenu.Show();
            return true;
        }
    }
}