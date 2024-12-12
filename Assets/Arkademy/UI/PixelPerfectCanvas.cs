using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.UI
{
    [RequireComponent(typeof(CanvasScaler))]
    [ExecuteInEditMode]
    public class PixelPerfectCanvas : MonoBehaviour
    {
        [SerializeField] private CanvasScaler cs;
        [SerializeField] private int pixelPerUnit;
        [SerializeField] private int pixelScale = 2;
        [SerializeField] private bool useReferedScale = true;
        [SerializeField] private Vector2 referenceResolution = new Vector2(270, 480);
        public int referedScale => Mathf.Min(Mathf.CeilToInt(Screen.width / referenceResolution.x),
            Mathf.CeilToInt(Screen.height / referenceResolution.y));
        private void Awake()
        {
            cs = GetComponent<CanvasScaler>();
            UpdateUsingSettings();
        }

        private void UpdateUsingSettings()
        {
            var setting = PixelPerfectSettings.GetSettings();
            if (!setting) return;
            pixelPerUnit = setting.pixelPerUnit;
            referenceResolution = setting.referenceResolution;
            pixelScale = setting.pixelScale;
            useReferedScale = setting.useReferredScale;
        }
        void Update()
        {
            cs.scaleFactor = useReferedScale ? referedScale : pixelScale;
            cs.referencePixelsPerUnit = pixelPerUnit;
        }
    }
}