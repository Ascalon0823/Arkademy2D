using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Arkademy.Rift.Editor
{
    [CustomPreview(typeof(RiftMap))]
    public class RiftMapPreview : ObjectPreview
    {
        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            var provider = target as RiftMap;
            if (provider)
                GUI.DrawTexture(r,provider.previewTexture);
            else base.OnPreviewGUI(r, background);
        }
    }
}