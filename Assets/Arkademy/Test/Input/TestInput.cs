using System;
using Arkademy.Gameplay;
using UnityEngine;

namespace Arkademy.Test.Input
{
    public class TestInput : MonoBehaviour
    {
        [SerializeField] private PlayerTouchInput input;

        [SerializeField] private DragIndicator dragIndicator;
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject touchIndicatorPrefab;

        private void Start()
        {
            input.onPressBegin += v =>
            {
                dragIndicator.gameObject.SetActive(true);
                var worldPos = camera.ScreenToWorldPoint(v);
                worldPos.z = 0;
                dragIndicator.transform.position = worldPos;
                dragIndicator.UpdatePos(worldPos, input.move.normalized, input.move);
            };
            input.onPressEnd += v => { dragIndicator.gameObject.SetActive(false); };
        }

        private void Update()
        {
            if (input.interact)
            {
                var worldPos = camera.ScreenToWorldPoint(input.screenPosition);
                worldPos.z = 0;
                Instantiate(touchIndicatorPrefab, worldPos, Quaternion.identity);
            }
            if (dragIndicator.gameObject.activeSelf)
                dragIndicator.UpdatePos(dragIndicator.transform.position, input.move.normalized, input.move);
        }
    }
}