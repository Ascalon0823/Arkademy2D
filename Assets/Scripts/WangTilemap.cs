using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Arkademy
{
    [RequireComponent(typeof(Tilemap))]
    public class WangTilemap : MonoBehaviour
    {
        [SerializeField] private Tile[] palette;

        [SerializeField] private Tilemap renderMap;
        public Vector2 offset;
        public Transform cameraPos;
        public int gridSize = 10;
        public Vector2Int gridPos;

        private void Start()
        {
            offset = Random.insideUnitCircle * Random.Range(1000, 5000);
            UpdateMap(true);
        }

        private void Update()
        {
            UpdateMap();
        }

        public void UpdateMap(bool force = false)
        {
            var newGridPos = new Vector2Int(Mathf.RoundToInt(cameraPos.position.x / gridSize),
                Mathf.RoundToInt(cameraPos.position.y / gridSize));
            if (newGridPos == gridPos && !force) return;
            gridPos = newGridPos;
            var size = gridSize * 3;
            var data = new int[size + 1, size + 1];
            var halfSize = size / 2;
            var origin = new Vector2Int(newGridPos.x * gridSize - halfSize, newGridPos.y * gridSize - halfSize);
            for (var y = 0; y <= size; y++)
            for (var x = 0; x <= size; x++)
            {
                data[x, y] =
                    Mathf.PerlinNoise((origin.x + x) * 8.0f / (size + 1) + offset.x,
                        (origin.y + y) * 8.0f / (size + 1) + offset.y) > 0.5
                        ? 1
                        : 0;
            }

            SetData(data, origin);
        }

        public void SetData(int[,] data, Vector2Int origin)
        {
            for (var y = 0; y < data.GetLength(1) - 1; y++)
            for (var x = 0; x < data.GetLength(0) - 1; x++)
            {
                var key = 0;
                key += data[x, y] << 0;
                key += data[x + 1, y] << 1;
                key += data[x, y + 1] << 2;
                key += data[x + 1, y + 1] << 3;
                renderMap.SetTile(
                    new Vector3Int(origin.x + x, origin.y + y, 0),
                    palette[key]);
            }
        }
    }
}