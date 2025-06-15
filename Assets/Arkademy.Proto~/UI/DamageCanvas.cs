using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.UI
{
    public class DamageCanvas : MonoBehaviour
    {
        
        private static DamageCanvas Canvas;

        [SerializeField] private DamageText textPrefab;
        private void Awake()
        {
            if (Canvas && Canvas != this)
            {
                Destroy(gameObject);
                return;
            }

            Canvas = this;
        }

        private DamageText InternalAddTextTo(Camera cam, Transform t, string content, DamageText prev)
        {
            var beginningPos = prev ?
                 //prev.beginningWorldPos + new Vector3(0, 0.35f) 
                 new Vector3(t.position.x, prev.beginningWorldPos.y + 0.35f)
                : (t.position + new Vector3(0, 0.6f));
            var text = Instantiate(textPrefab, cam.WorldToScreenPoint(beginningPos), Quaternion.identity, transform);
            text.beginningWorldPos = beginningPos;
            text.cam = cam;
            text.content = content;
            return text;
        }
        public static void AddTextTo(Camera cam, Transform t, string text)
        {
            if (!Canvas)
            {
                Canvas = Instantiate(Resources.Load<DamageCanvas>("DamageCanvas"));
            }
            Canvas.InternalAddTextTo(cam,t,text,null);
        }

        public void StartAddingText(Camera cam, Transform t, string[] text)
        {
            StartCoroutine(AddTexts(cam, t, text));
        }
        private IEnumerator AddTexts(Camera cam, Transform t, string[] texts)
        {
            DamageText prev = null;
            foreach (var text in texts)
            {
                prev = InternalAddTextTo(cam, t, text, prev);
                yield return new WaitForSeconds(0.1f);
            }
        }
        public static void AddTextTo(Camera cam, Transform t, string[] text)
        {
            if (!Canvas)
            {
                Canvas = Instantiate(Resources.Load<DamageCanvas>("DamageCanvas"));
            }
            Canvas.StartAddingText(cam,t,text);
        }
    }
}