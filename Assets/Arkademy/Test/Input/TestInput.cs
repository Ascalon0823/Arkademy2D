using System;
using Arkademy.Gameplay;
using UnityEngine;

namespace Arkademy.Test.Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class TestInput : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;

        [SerializeField] private DragIndicator dragIndicator;
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject touchIndicatorPrefab;

        private void Start()
        {
            input.onFire += v =>
            {
                Debug.Log($"OnFire: {v}");
                var worldPos = camera.ScreenToWorldPoint(v);
                worldPos.z = 0;
                Instantiate(touchIndicatorPrefab, worldPos, Quaternion.identity);
            };
            input.onPressBegin += v =>
            {
                dragIndicator.gameObject.SetActive(true);
                var worldPos = camera.ScreenToWorldPoint(v);
                worldPos.z = 0;
                dragIndicator.transform.position = worldPos;
                dragIndicator.UpdatePos(worldPos, input.moveDir, input.moveAnalog, worldPos);
            };
            input.onPressEnd += v => { dragIndicator.gameObject.SetActive(false); };
        }

        private void Update()
        {
            if (dragIndicator.gameObject.activeSelf)
                dragIndicator.UpdatePos(dragIndicator.transform.position, input.moveDir, input.moveAnalog,
                    camera.ScreenToWorldPoint(input.moveRaw)-camera.ScreenToWorldPoint(Vector2.zero));
        }
    }
}