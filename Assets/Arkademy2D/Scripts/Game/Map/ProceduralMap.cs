using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Arkademy2D.Game.Map
{
    public class ProceduralMap : TileMap
    {
        public Tilemap floorMap;
        public Tilemap wallMap;

        public TileBase floor;
        public TileBase wall;
        public int roomsCount;
        public int roomSize;
        public bool generated;

        private void Start()
        {
            Generate();
        }

        public void Generate()
        {
            if (generated) return;
            var dirs = new Vector2Int[]
            {
                Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
            };
            var rooms = new List<Vector2Int>();
            var floorTiles = new List<TileBase>();
            var floorPoses = new List<Vector3Int>();
            var wallTiles = new List<TileBase>();
            var wallPoses = new List<Vector3Int>();
            for (var i = 0; i < roomsCount; i++)
            {
                var roomCoord = Vector2Int.zero;
                if (rooms.Count != 0)
                {
                    var selected = false;
                    while (!selected)
                    {
                        var fromRoom = rooms[Random.Range(0, rooms.Count)];
                        var toRoom = fromRoom + dirs[Random.Range(0, dirs.Length)];
                        if (rooms.Contains(toRoom))
                        {
                            continue;
                        }
                        roomCoord = toRoom;
                        selected = true;
                    }
                }
                rooms.Add(roomCoord);
                var xRoot = roomCoord.x*roomSize - roomSize/2;
                var yRoot = roomCoord.y*roomSize - roomSize/2;
                for(var x = 0; x < roomSize; x++)
                for (var y = 0; y < roomSize; y++)
                {
                    var isWall = x == 0 || x == roomSize - 1 || y == 0 || y == roomSize - 1;
                    var pos = isWall ? wallPoses :  floorPoses;
                    var tiles = isWall ? wallTiles : floorTiles;
                    pos.Add(new Vector3Int(x+xRoot, y+yRoot, 0));
                    tiles.Add(isWall?wall : floor);
                }
            }
            floorMap.SetTiles(floorPoses.ToArray(), floorTiles.ToArray());
            wallMap.SetTiles(wallPoses.ToArray(), wallTiles.ToArray());
            generated = true;
        }
    }
}