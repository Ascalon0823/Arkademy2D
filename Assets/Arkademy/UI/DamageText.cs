using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Arkademy.UI
{
    public class DamageText : MonoBehaviour
    {
        public string content;
        public float life;
        public float maxLife;
        public float speed;
        public Color color;
        public Vector3 beginningWorldPos;
        public Camera cam;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float horizontalOffsetRange;
        [SerializeField] private Vector3 horizontalOffset;

        private void Start()
        {
            text.text = content;
            horizontalOffset = new Vector3(Random.Range(-horizontalOffsetRange, horizontalOffsetRange), 0, 0);
            text.color = color;
            if (cam)
                transform.position = cam.WorldToScreenPoint(beginningWorldPos + horizontalOffset);
        }

        private void Update()
        {
            if (life < 0)
            {
                Destroy(gameObject);
                return;
            }

            text.text = content;
            life -= Time.deltaTime;
            text.color = color;
            text.alpha = life / maxLife;
            beginningWorldPos += Time.deltaTime * new Vector3(0, speed);
            if (cam)
                transform.position = cam.WorldToScreenPoint(beginningWorldPos + horizontalOffset);
        }
    }
}