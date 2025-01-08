using System;
using Arkademy.Gameplay;
using UnityEngine;

namespace Arkademy.Test.Input
{
    public class TestInput : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject touchIndicatorPrefab;
        [SerializeField] private GameObject holdIndicator;
        [SerializeField] private LineRenderer holdLine;

        private void Start()
        {
        }

        private void Update()
        {
            if (input.interact)
            {
                var worldPos = camera.ScreenToWorldPoint(input.position);
                worldPos.z = 0;
                Instantiate(touchIndicatorPrefab, worldPos, Quaternion.identity);
            }

            holdIndicator.gameObject.SetActive(input.hold);
            
            var start = camera.ScreenToWorldPoint(input.holdPos);
            start.z = 0;
            var end = camera.ScreenToWorldPoint(input.holdPos + input.holdDir);
            end.z = 0f;
            holdIndicator.transform.position = end;
            holdLine.SetPositions(new[] { start, end });
        }
    }
}