using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arkademy.Gameplay
{
    public class Timer : MonoBehaviour
    {
        public float time;
        public float remaining;
        public UnityEvent OnTimerEnd;
        public bool resetOnStart;

        private void Start()
        {
            if(resetOnStart)ResetTimer();
        }

        public void ResetTimer()
        {
            remaining = time;
        }

        private void Update()
        {
            if (remaining <= 0) return;
            remaining -= Time.deltaTime;
            if (remaining <= 0)
            {
                OnTimerEnd?.Invoke();
            }
        }
    }
}