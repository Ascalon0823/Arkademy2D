using System;
using System.Linq;
using UnityEngine;

namespace Arkademy2D.Game.Interaction
{
    public class Detector : MonoBehaviour
    {
        public Interactable candidate;
        public float radius;
        private void FixedUpdate()
        {
            candidate = Physics2D.OverlapCircleAll(transform.position, radius)?
                .Select(x => x.GetComponent<Interactable>())?
                .Where(x => x)?
                .OrderBy(x => Vector2.Distance(x.transform.position, transform.position))?
                .FirstOrDefault();
        }
    }
}