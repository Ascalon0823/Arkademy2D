using System;
using UnityEngine;

namespace Arkademy2D.Game.Behaviours.Actor
{
    public class Energy : MonoBehaviour
    {
        public int max;
        public int current;
        public float currentFloat;
        public float regenRate;
        public void Update()
        {
            currentFloat = Mathf.Clamp(currentFloat + max * regenRate * Time.deltaTime, 0f, max);
            current = Mathf.FloorToInt(currentFloat);
        }

        public bool TrySpend(int amount)
        {
            if (currentFloat < amount) return false;
            current -= amount;
            current = Mathf.Clamp(current, 0, max);
            return true;
        }
    }
}