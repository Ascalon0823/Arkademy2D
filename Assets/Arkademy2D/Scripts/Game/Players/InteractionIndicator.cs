using System;
using UnityEngine;
using Arkademy2D.Game.Interaction;
namespace Arkademy2D.Game.Player
{
    public class InteractionIndicator : MonoBehaviour
    {
        public Detector playerDetector;

        public GameObject indicator;

        private void LateUpdate()
        {
            indicator.SetActive(playerDetector.candidate);
            if (!playerDetector.candidate) return;
            indicator.transform.position = playerDetector.candidate.transform.position;
        }
    }
}