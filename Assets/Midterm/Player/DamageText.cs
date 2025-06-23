using System;
using TMPro;
using UnityEngine;

namespace Midterm.Player
{
    public class DamageText : MonoBehaviour
    {
       
        public float time;
        public float remainingTime;
        public TextMeshProUGUI text;
        public void Start()
        {
            remainingTime = time;
        }

        public void LateUpdate()
        {
            remainingTime -= Time.deltaTime;
            text.alpha = Mathf.Clamp01(remainingTime / time);
        }
    }
}