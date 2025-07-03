using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Midterm.Player
{
    public class DamageTextGroup : MonoBehaviour
    {
        public RectTransform content;

        public DamageText prefab;
        public float life;
        public float remainingTime;
        public float speed;

        public void AddDamage(int damage)
        {
            var text = Instantiate(prefab, content);
            text.text.transform.localPosition += new Vector3(Random.Range(-4f, 4f), 0, 0);
            text.remainingTime = life;
            remainingTime = life;
            text.text.text = damage.ToString();
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