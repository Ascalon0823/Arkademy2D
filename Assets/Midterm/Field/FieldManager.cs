using System;
using System.Collections.Generic;
using ArkademyStudio.PixelPerfect;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Midterm.Field
{
    public class FieldManager : MonoBehaviour
    {
        public Tilemap tilemap;
        public WangTile.WangTile wangTile;
        public GameObject pillarPrefab;
        public int seed;
        public int xOff;
        public int yOff;
        [Range(1,8)]
        public int scale;

        [Range(0f, 1f)] public float pillarPercent;

        public int chunkSize;
        public PixelPerfectCamera refCam;
        public Vector2Int lastLowerChunk;
        public Vector2Int lastUpperChunk;
        public HashSet<Vector2Int> froms = new HashSet<Vector2Int>();
        

        private void Start()
        {
            seed = Random.Range(0, 10000);
            Random.InitState(seed);
            xOff = Random.Range(-10000, 10000);
            yOff = Random.Range(-10000, 10000);
        }

        public void Update()
        {
            var lower = new Vector2(refCam.camRect.xMin, refCam.camRect.yMin);
            var upper = new Vector2(refCam.camRect.xMax, refCam.camRect.yMax);
            var lowerChunk =
                new Vector2Int(Mathf.FloorToInt(lower.x / chunkSize), Mathf.FloorToInt(lower.y / chunkSize)) -
                Vector2Int.one;
            var upperChunk =
                new Vector2Int(Mathf.CeilToInt(upper.x / chunkSize), Mathf.CeilToInt(upper.y / chunkSize)) +
                Vector2Int.one;
            if (lowerChunk == lastLowerChunk && upperChunk == lastUpperChunk) return;
            lastLowerChunk = lowerChunk;
            lastUpperChunk = upperChunk;
            for(var i =lowerChunk.x;i<upperChunk.x;i++)
            for (var j = lowerChunk.y; j < upperChunk.y; j++)
            {
                var from = new Vector2Int(i*chunkSize, j*chunkSize);
                if (froms.Contains(from)) continue;
                var to = new Vector2Int((i+1)*chunkSize, (j+1)*chunkSize);
                Build(from, to);
                froms.Add(from);
            }
        }

        public void Build(Vector2Int from, Vector2Int to)
        {
            var width = to.x - from.x;
            var height = to.y - from.y;
            var data = new int[width + 1][];
            for (int index = 0; index < width + 1; index++)
            {
                data[index] = new int[height + 1];
            }

            for (var i = 0; i <= width; i++)
            for (var j = 0; j <= height; j++)
            {
                var coordX = from.x + i;
                var coordY = from.y + j;
                var p = Mathf.PerlinNoise(xOff + coordX / 100f * scale, yOff + coordY / 100f * scale);
                var pillar = Random.Range(0f, 1f) < pillarPercent;
                if (pillar)
                {
                    Instantiate(pillarPrefab, new Vector3(coordX + 0.5f, coordY + 0.5f, 0), Quaternion.identity,
                        transform);
                }

                data[i][j] = p > 0.5f ? 1 : 0;
                if (i > 0 && j > 0)
                {
                    var idx = 0;
                    idx += data[i - 1][j - 1] << 0;
                    idx += data[i][j - 1] << 1;
                    idx += data[i - 1][j] << 2;
                    idx += data[i][j] << 3;
                    tilemap.SetTile(new Vector3Int(coordX - 1, coordY - 1, 0), wangTile.tiles[idx]);
                }
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var from in froms)
            {
                Gizmos.DrawWireCube(new Vector3(from.x + chunkSize/2f, from.y + chunkSize/2f, 0), Vector3.one * chunkSize);
            }
        }
    }
}