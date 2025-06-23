using System;
using System.Collections;
using UnityEngine;

namespace Midterm.Player
{
    public class DamageTextGroup : MonoBehaviour
    {
        public RectTransform content;

        public DamageText prefab;
        public int[] damages;
        public float life;
        public float remainingTime;
        public float speed;

        public void Start()
        {
            StartCoroutine(SpawnText());
        }

        private IEnumerator SpawnText()
        {
            foreach (var d in damages)
            {
                var text = Instantiate(prefab, content);
                text.remainingTime = life;
                remainingTime = life;
                text.text.text = d.ToString();
                yield return new WaitForSeconds(0.2f);
            }
        }
        private void LateUpdate()
        {
            remainingTime -= Time.deltaTime;
            transform.position += Vector3.up * speed * Time.deltaTime;
            if (remainingTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}