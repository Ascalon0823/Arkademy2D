using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public float lifeTime;
        public int childLimit;

        public void AddText(int text)
        {
            var spawn = Instantiate(textPrefab, transform);
            lifeTime = Mathf.Max(lifeTime, spawn.lifeTime);
            spawn.text.text = text.ToString();
            spawnedText.Add(spawn);
        }
        private void LateUpdate()
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0f)
            {
                Destroy(gameObject);
            }

            if (followTarget)
            {
                basePosition = PlayerBehaviour.Player.playerCamera.ppcam.cam.WorldToScreenPoint(followTarget.position);
            }

            yPos += flyUpSpeed * Time.deltaTime;
            transform.position = basePosition + new Vector3(0, yPos, 0);
        }
    }
}