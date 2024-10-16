using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Arkademy
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Tilemap))]
    public class WangTilemap : MonoBehaviour
    {
        [SerializeField] private Tile[] palette;

        [SerializeField] private Tilemap renderMap;
        public Vector2 offset;
        public PixelPerfectCamera cam;
        public int gridSize = 10;
        public Vector2Int gridPos;

        [SerializeField] private Tilemap useReferenceMap;
        [SerializeField] private Sprite base0;
        [SerializeField] private Sprite base1;

        private void OnEnable()
        {
            if (useReferenceMap && Application.isEditor)
            {
                Tilemap.tilemapTileChanged += UpdateTileMapBasedOnReferenceMap;
            }
        }

        private void OnDisable()
        {
            if (useReferenceMap && Application.isEditor)
            {
                Tilemap.tilemapTileChanged -= UpdateTileMapBasedOnReferenceMap;
            }
        }

        private void Start()
        {
            offset = Random.insideUnitCircle * Random.Range(1000, 5000);
            if (!Application.isEditor && cam) UpdateMapUsingCamPos(true);
            if (useReferenceMap)
            {
                var refRenderer = useReferenceMap.gameObject.GetComponent<TilemapRenderer>();
                if (refRenderer && !Application.isEditor) refRenderer.enabled = false;
            }
        }

        private void UpdateTileMapBasedOnReferenceMap(Tilemap map, Tilemap.SyncTile[] syncTiles)
        {
            if (map != useReferenceMap) return;
            foreach (var tile in syncTiles)
            {
                var toBeUpdated = new[]
                {
                    tile.position, tile.position - new Vector3Int(1, 0), tile.position - new Vector3Int(0, 1),
                    tile.position - new Vector3Int(1, 1)
                };
                foreach (var pos in toBeUpdated)
                {
                    var corners = new[]
                        { pos, pos + new Vector3Int(1, 0), pos + new Vector3Int(0, 1), pos + new Vector3Int(1, 1) };
                    var key = 0;
                    for (var i = 0; i < 4; i++)
                    {
                        var refTile = useReferenceMap.GetTile(corners[i]);
                        var value = 0;
                        if (refTile)
                        {
                            var data = new TileData();
                            refTile.GetTileData(corners[i], useReferenceMap, ref data);
                            value = data.sprite == base0 ? 1 : 0;
                        }

                        key += value << i;
                    }

                    renderMap.SetTile(pos, palette[key]);
                }
            }
        }

        private void Update()
        {
            if (cam)
                UpdateMapUsingCamPos();
        }


        public void UpdateMapUsingCamPos(bool force = false)
        {
            gridSize = Mathf.CeilToInt(cam.camOrthoSize * 2);
            var newGridPos = new Vector2Int(Mathf.RoundToInt(cam.transform.position.x / gridSize),
                Mathf.RoundToInt(cam.transform.position.y / gridSize));
            if (newGridPos == gridPos && !force) return;
            gridPos = newGridPos;
            var size = gridSize * 3;
            var data = new int[size + 1, size + 1];
            var halfSize = size / 2;
            var origin = new Vector2Int(newGridPos.x * gridSize - halfSize, newGridPos.y * gridSize - halfSize);
            for (var y = 0; y <= size; y++)
            for (var x = 0; x <= size; x++)
            {
                if (useReferenceMap)
                {
                    var pos = new Vector3Int(origin.x + x, origin.y + y, 0);
                    var colliderMapData = useReferenceMap.GetTile(pos);
                    if (!colliderMapData)
                    {
                        data[x, y] = 1;
                        continue;
                    }

                    var tileData = new TileData();
                    colliderMapData.GetTileData(pos, useReferenceMap, ref tileData);
                    data[x, y] = tileData.sprite == base1 ? 1 : 0;
                    continue;
                }

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
                if (data[x, y] == -1 || data[x + 1, y] == -1 || data[x, y + 1] == -1 ||
                    data[x + 1, y + 1] == -1) continue;
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