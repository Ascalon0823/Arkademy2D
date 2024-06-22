using System;
using UnityEngine;

namespace Arkademy
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PixelPerfectCamera : MonoBehaviour
    {
        [Header("Camera")] [Range(1, 16)] public int pixelScale = 2;
        public bool useReferedScale = true;
        public Vector2 referenceResolution = new Vector2(270, 480);
        public int referedScale => Mathf.Min(Mathf.CeilToInt(Screen.width / referenceResolution.x),
            Mathf.CeilToInt(Screen.height / referenceResolution.y));
        public int pixelPerUnit = 16;
        public Camera cam;
        public float camOrthoSize => Screen.height / (pixelScale * pixelPerUnit * 2.0f);
        public Rect camRect;

        protected virtual void Awake()
        {
            cam = GetComponent<Camera>();
            cam.orthographic = true;
            Application.targetFrameRate = 60;
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

        public Vector2 GetWorldPos(Vector2 screenPos)
        {
            return cam.ScreenToWorldPoint(screenPos);
        }

        protected virtual void Update()
        {
            if (!cam) return;
            if (useReferedScale) pixelScale = referedScale;
            cam.orthographicSize = camOrthoSize;
            camRect.center = cam.transform.position;
            camRect.size = new Vector2(cam.aspect, 1f) * (cam.orthographicSize * 2f);
        }

        private void OnDrawGizmos()
        {
            if (!cam) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLineStrip(new ReadOnlySpan<Vector3>(new[]
            {
                new Vector3(camRect.xMin, camRect.yMin, 0),
                new Vector3(camRect.xMin, camRect.yMax, 0),
                new Vector3(camRect.xMax, camRect.yMax, 0),
                new Vector3(camRect.xMax, camRect.yMin, 0),
            }), true);
        }
    }
}