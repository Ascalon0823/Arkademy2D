using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkademy.Rift
{
    [Serializable]
    public struct AutomataConfig
    {
        public int seed;
        public int roomSize;
        public int openingSize;
        [Range(0, 1f)] public float percentage;
        public List<AutomataIteration> iterations;
    }

    [Serializable]
    public struct AutomataIteration
    {
        public int iterations;
        [Range(-1, 4)] public int minCondition;
        [Range(1, 10)] public int minNeighbourRange;
        [Range(4, 9)] public int maxCondition;
        [Range(1,10)] public int maxNeighbourRange;
    }

    public class RiftRoom
    {
        public RiftRoom[] Connected = new RiftRoom[4];
        public Vector2Int Position;
        public int[,] Cells;
        public AutomataConfig config;
        public List<Vector2Int> OpenArea = new List<Vector2Int>();

        public Vector2Int GetRandomOpenArea()
        {
            return OpenArea[Random.Range(0, OpenArea.Count)];
        }

        public Vector3 WorldPos(Vector2Int coord)
        {
            var worldCoord = Position * config.roomSize + coord;
            return new Vector3(worldCoord.x, worldCoord.y);
        }

        public static int CountNeighbours(int x, int y, int range, int[,] cell, int size)
        {
            var count = 0;
            for(var i=x-range;i<=x+range;i++)
            for (var j = y - range; j <= y + range; j++)
            {
                if (i == x && j == y) continue;
                if (i < 0 || i > size - 1 || j < 0 || j > size - 1 || cell[i, j] == 1)
                {
                    count++;
                }
            }
            return count;
        }
        public void Automata()
        {
            foreach (var iteration in config.iterations)
            {
                for (var i = 0; i < iteration.iterations; i++)
                {
                    var temp = new int[config.roomSize, config.roomSize];
                    for (var x = 0; x < config.roomSize; x++)
                    for (var y = 0; y < config.roomSize; y++)
                    {

                        temp[x, y] = CountNeighbours(x,y,iteration.maxNeighbourRange,Cells,config.roomSize) >= iteration.maxCondition ||
                                     CountNeighbours(x,y,iteration.minNeighbourRange,Cells,config.roomSize)  <= iteration.minCondition
                            ? 1
                            : 0;
                    }

                    Cells = temp;
                }
            }
        }

        public void Fill()
        {
            Random.InitState(config.seed);
            Cells = new int[config.roomSize, config.roomSize];
            for (var x = 0; x < config.roomSize; x++)
            for (var y = 0; y < config.roomSize; y++)
            {
                Cells[x, y] = Random.Range(0f, 1f) < config.percentage ? 1 : 0;
                Cells[x, y] = x == 0 || x == config.roomSize - 1 || y == 0 || y == config.roomSize - 1
                    ? 1
                    : Cells[x, y];
            }

            for (var dir = 0; dir < RiftMap.Directions.Count; dir++)
            {
                if (Connected[dir] == null) continue;
                var center = new Vector2Int(config.roomSize / 2, config.roomSize / 2);
                var off = config.roomSize / 2 * RiftMap.Directions[dir];
                var from = center + off - config.openingSize * Vector2Int.one;
                var to = center + off + config.openingSize * Vector2Int.one;
                for (var x = from.x; x <= to.x; x++)
                for (var y = from.y; y <= to.y; y++)
                {
                    if (x < 0 || x > config.roomSize - 1 || y < 0 || y > config.roomSize - 1) continue;
                    Cells[x, y] = 0;
                }
            }

            Automata();
            for (var x = 0; x < config.roomSize; x++)
            for (var y = 0; y < config.roomSize; y++)
            {
                if (Cells[x, y] == 1) continue;
                OpenArea.Add(new Vector2Int(x, y));
            }
        }
    }

    public class RiftMap : MonoBehaviour
    {
        public int roomCount;
        public Vector3 startPos;
        public AutomataConfig config;
        public bool preview;

        public static readonly List<Vector2Int> Directions = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        public static readonly List<Vector2Int> Directions8 = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.right,
            Vector2Int.right + Vector2Int.down,
            Vector2Int.down,
            Vector2Int.down + Vector2Int.left,
            Vector2Int.left,
            Vector2Int.left + Vector2Int.up,
        };

        public Dictionary<Vector2Int, RiftRoom> Rooms = new Dictionary<Vector2Int, RiftRoom>();
        public Texture2D previewTexture;

        public void Generate()
        {
            Rooms.Clear();
            Random.InitState(config.seed);
            var pos = Vector2Int.zero;
            RiftRoom prev = null;
            for (var i = 0; i < roomCount; i++)
            {
                var room = new RiftRoom
                {
                    Position = pos,
                    config = config
                };
                room.config.seed = Random.Range(int.MinValue, int.MaxValue);
                Rooms.Add(pos, room);
                if (prev != null)
                {
                    var dir = prev.Position - pos;
                    var dirIdx = Directions.IndexOf(dir);
                    room.Connected[dirIdx] = prev;
                    var oppositeIdx = Directions.IndexOf(-dir);
                    prev.Connected[oppositeIdx] = room;
                    room.Cells = new int[config.roomSize, config.roomSize];
                }

                prev = room;
                var candidate = Directions.Where(x => !Rooms.ContainsKey(x + pos)).ToList();
                if (!candidate.Any()) break;

                pos += candidate[Random.Range(0, candidate.Count)];
            }

            foreach (var room in Rooms.Values)
            {
                room.Fill();
            }

            var startRoom = Rooms[Vector2Int.zero];
            startPos = startRoom.WorldPos(startRoom.GetRandomOpenArea());
        }

        protected virtual void OnValidate()
        {
            if (!preview) return;
            if (!previewTexture)
            {
                previewTexture = new Texture2D(config.roomSize, config.roomSize);
            }

            previewTexture.Reinitialize(config.roomSize, config.roomSize);
            var data = new int[config.roomSize, config.roomSize];
            Random.InitState(config.seed);
            for (var x = 0; x < config.roomSize; x++)
            for (var y = 0; y < config.roomSize; y++)
            {
                data[x, y] = Random.Range(0f, 1f) > config.percentage ? 0 : 1;
                data[x, y] = x == 0 || x == config.roomSize - 1 || y == 0 || y == config.roomSize - 1 ? 1 : data[x, y];
            }

            foreach (var iteration in config.iterations)
            {
                for (var i = 0; i < iteration.iterations; i++)
                {
                    var temp = new int[config.roomSize, config.roomSize];
                    for (var x = 0; x < config.roomSize; x++)
                    for (var y = 0; y < config.roomSize; y++)
                    {
                        

                        temp[x, y] = RiftRoom.CountNeighbours(x,y,iteration.maxNeighbourRange,data,config.roomSize) >= iteration.maxCondition ||
                                     RiftRoom.CountNeighbours(x,y,iteration.minNeighbourRange,data,config.roomSize)  <= iteration.minCondition
                            ? 1
                            : 0;
                    }

                    data = temp;
                }
            }


            for (var i = 0; i < config.roomSize; i++)
            for (var j = 0; j < config.roomSize; j++)
            {
                previewTexture.SetPixel(i, j, data[i, j] == 1 ? Color.black : Color.white);
            }

            previewTexture.Apply();
        }
    }
}