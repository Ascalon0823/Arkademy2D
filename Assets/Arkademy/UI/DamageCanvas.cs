using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.UI
{
    public class DamageCanvas : MonoBehaviour
    {
        public static DamageCanvas Canvas;

        [SerializeField] private DamageText textPrefab;
        private Dictionary<Transform, DamageText> prevs = new Dictionary<Transform, DamageText>();
        private void Awake()
        {
            if (Canvas && Canvas != this)
            {
                Destroy(gameObject);
                return;
            }

            Canvas = this;
        }

        private void InternalAddTextTo(Camera cam, Transform t, string content, bool newGroup = false)
        {
            if (newGroup) prevs[t] = null;
            var prev = prevs.GetValueOrDefault(t);
            var beginningPos = prev ?
                 //prev.beginningWorldPos + new Vector3(0, 0.35f) 
                 new Vector3(t.position.x, prev.beginningWorldPos.y + 0.35f)
                : (t.position + new Vector3(0, 0.6f));
            var text = Instantiate(textPrefab, cam.WorldToScreenPoint(beginningPos), Quaternion.identity, transform);
            text.beginningWorldPos = beginningPos;
            text.cam = cam;
            text.content = content;
            prevs[t] = text;
        }
        public static void AddTextTo(Camera cam, Transform t, string text, bool newGroup = false)
        {
            Canvas.InternalAddTextTo(cam,t,text,newGroup);
        }
    }
}