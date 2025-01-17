using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy
{
    public class FollowCamera : MonoBehaviour
    {
        public PixelPerfectCamera ppcam;
        public Vector2 offsetPercentage;
        public Transform followTarget;

        private void Update()
        {
            if (!followTarget) return;
            transform.position = followTarget.position + new Vector3(offsetPercentage.x * ppcam.camRect.width * 0.5f,
                offsetPercentage.y * ppcam.camRect.height* 0.5f, -10f);
        }

        public Vector2 GetRandomPosOutsideViewport(float distance)
        {
            var radius = Mathf.Max(ppcam.camRect.width, ppcam.camRect.height)/2f;
            var random = Random.insideUnitCircle;
            return (Vector2)followTarget.transform.position + random.normalized * radius;
        }
    }
}