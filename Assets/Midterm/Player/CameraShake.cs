using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Midterm.Player
{
    public class CameraShake : MonoBehaviour
    {
        public float shakeStrength;

        [SerializeField] private float shakeTime;
        [SerializeField] private float remainingShake;

        [ContextMenu("Shake")]
        public void Shake(float str, float time)
        {
            
            if(str>=shakeStrength)shakeStrength = str;
            if (remainingShake < time) shakeTime = time;
            if (remainingShake <= 0)
            {
                shakeStrength = str;
                shakeTime = time;
            }
            remainingShake = shakeTime;
        }

        private void LateUpdate()
        {
            remainingShake = Mathf.Clamp(remainingShake - Time.deltaTime, 0.0f, shakeTime);
            transform.localPosition = Random.insideUnitCircle * shakeStrength * Mathf.Clamp01(remainingShake);
        }
    }
}