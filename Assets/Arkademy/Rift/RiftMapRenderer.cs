using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Arkademy.Rift
{
    public class RiftMapRenderer : MonoBehaviour
    {
        public RiftMap map;
        public Tilemap floor;
        public Tilemap wall;
        public TileBase[] palette;

        public void Render()
        {
            var tiles = new List<TileBase>();
            var positions = new List<Vector3Int>();
            var floorTiles = new List<TileBase>();
            var floorPos = new List<Vector3Int>();
            foreach (var pair in map.Rooms)
            {
                for(var x =0; x < map.roomSize; x++)
                for (var y = 0; y < map.roomSize; y++)
                {
                    var pos = new Vector2Int(x, y) + pair.Key * map.roomSize;
                    var worldPos = new Vector3Int(pos.x, pos.y, 0);
                    var value = pair.Value.Cells[x, y];
                    var tile = palette[value];
                    (value == 0 ? floorTiles : tiles).Add(tile);
                    (value == 0 ? floorPos : positions).Add(worldPos);
                }
            }
            floor.ClearAllTiles();
            floor.SetTiles(floorPos.ToArray(), floorTiles.ToArray());
            wall.ClearAllTiles();
            wall.SetTiles(positions.ToArray(), tiles.ToArray());
        }
    }
}