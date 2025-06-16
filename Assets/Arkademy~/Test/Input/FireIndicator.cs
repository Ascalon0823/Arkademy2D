using System;
using UnityEngine;

namespace Arkademy.Test.Input
{
    public class FireIndicator : MonoBehaviour
    {
        [SerializeField] private float time;
        private float maxTime;

        private void Start()
        {
            transform.localScale = Vector3.zero;
            maxTime = time;
        }

        private void Update()
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                Destroy(gameObject);
                return;
            }

            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time / maxTime);
        }
    }
}