using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arkademy.UI.Game
{
    public class DamageTextGroup : MonoBehaviour
    {
        [SerializeField] private DamageTextLine textPrefab;
        public List<DamageTextLine> spawnedText = new();
        public Transform followTarget;
        [SerializeField] private Vector3 basePosition;
        [SerializeField] private float yPos;
        [SerializeField] private float flyUpSpeed;
        public float remainingTime;
        public int childLimit;
        public Vector2 spawnOffset;
        public float totalLifeTime;

        public int dealerInstance;
        public int batch;

        public void AddText(DamageEvent de)
        {
            var spawn = Instantiate(textPrefab, transform);
            remainingTime = Mathf.Max(remainingTime, spawn.lifeTime);
            dealerInstance = de.dealerInstance;
            batch = de.batch;
            spawn.text.text = de.amount.ToString();
            spawnedText.Add(spawn);
        }

        private void LateUpdate()
        {
            remainingTime -= Time.deltaTime;
            totalLifeTime += Time.deltaTime;
            if (remainingTime <= 0f)
            {
                Destroy(gameObject);
            }

            if (followTarget)
            {
                basePosition =
                    PlayerBehaviour.PlayerCam.cam.WorldToScreenPoint(followTarget.position +
                        (Vector3)spawnOffset);
            }

            yPos += flyUpSpeed * Time.deltaTime;
            transform.position = basePosition + new Vector3(0, yPos, 0);
        }
    }
}