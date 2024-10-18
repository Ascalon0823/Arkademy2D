using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Arkademy.UI.Game
{
    public class DamageTextLine : MonoBehaviour
    {
        public TextMeshProUGUI text;

        public float lifeTime;

        [SerializeField] private float remaingLife;

        private void Start()
        {
            remaingLife = lifeTime;
        }

        private void Update()
        {
            if (remaingLife <= 0) return;
            remaingLife -= Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Clamp01(remaingLife / lifeTime));
        }
    }
}