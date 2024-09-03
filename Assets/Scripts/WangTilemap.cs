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


        private void Start()
        {
            var data = new int[101, 101];
            var offset = Random.insideUnitCircle * Random.Range(1000, 5000);
            
            for (var y = 0; y < 101; y++)
            for (var x = 0; x < 101; x++)
            {
                data[x, y] = Mathf.PerlinNoise(x * 8.0f / 101 + offset.x, y * 8.0f / 101 + offset.y) > 0.5 ? 1 : 0;
            }

            SetData(data);
        }

        public void SetData(int[,] data)
        {
            for (var y = 0; y < data.GetLength(1) - 1; y++)
            for (var x = 0; x < data.GetLength(0) - 1; x++)
            {
                var key = 0;
                key += data[x, y] << 0;
                key += data[x+1, y] << 1;
                key += data[x, y+1] << 2;
                key += data[x + 1, y + 1] << 3;
                renderMap.SetTile(new Vector3Int(x, y, 0), palette[key]);
            }
        }
    }
}