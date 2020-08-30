using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FloorMatrix : MonoBehaviour
    {
        public const int maxFloorSize = 10;

        //main matrix holding all floor data
        FloorTile[,] matrix = new FloorTile[maxFloorSize, maxFloorSize];

        float totalRooms = 0f;

        public Color[] colors;

        private void Awake()
        {

        }

        void Start()
        {
            FloorManager.Instance.SetMatrix(this);
            GenerateMatrix();
        }

        public FloorTile[,] GetMatrix() { return matrix; }

        public int[,] GetIntMatrix()
        {
            int[,] returnArr = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    returnArr[x, y] = matrix[x, y].index;
                }
            }
            return returnArr;
        }

        void GenerateMatrix()
        {
            SpawnInitalRoom(5, 5);
        }

        void SpawnInitalRoom(int x, int y)
        {
            matrix[x, y] = new FloorTile();
            matrix[x, y].index = FloorManager.Instance.GetRandomFloor1MapIndex();
            if (totalRooms < colors.Length) { matrix[x, y].color = colors[(int)totalRooms]; }

            int averageChain = 6;
            totalRooms++;
            for (int i = 0; i < 4; i++)
            {
                if (Random.Range(0f, 100f) > 33f)
                {
                    matrix[x, y].connections[i] = true;
                }
            }
            int n = matrix[x, y].TotalNeighbors();
            while (n < 2)
            {
                matrix[x, y].connections[Random.Range(0, 4)] = true;
                n = matrix[x, y].TotalNeighbors();
            }
            for (int i = 0; i < matrix[x, y].connections.Length; i++)
            {
                if (matrix[x, y].connections[i])
                {
                    int opposite = i + 2 < 4 ? i + 2 : i - 2;
                    if (matrix[x + (int)matrix[x, y].connectionMults[i].x, y + (int)matrix[x, y].connectionMults[i].y] == null)
                        SpawnRoom(x + (int)matrix[x, y].connectionMults[i].x, y + (int)matrix[x, y].connectionMults[i].y, averageChain).connections[opposite] = true;
                    else
                        matrix[x + (int)matrix[x, y].connectionMults[i].x, y + (int)matrix[x, y].connectionMults[i].y].connections[opposite] = true;
                }
            }
        }

        FloorTile SpawnRoom(int x, int y, int itter)
        {
            FloorTile tile = new FloorTile();
            matrix[x, y] = tile;
            tile.index = FloorManager.Instance.GetRandomFloor1MapIndex();
            if (totalRooms < colors.Length)
            {
                tile.color = colors[(int)totalRooms];
            }

            totalRooms++;

            if (x + 1 < maxFloorSize && Random.Range(0f, 100f) < ((float)itter / 8f) * 100f)
            {
                if (matrix[x + 1, y] == null)
                {
                    SpawnRoom(x + 1, y, itter - 1).connections[3] = true;
                    tile.connections[1] = true;
                }
            }
            if (y + 1 < maxFloorSize && Random.Range(0f, 100f) < ((float)itter / 8f) * 100f)
            {
                if (matrix[x, y + 1] == null)
                {
                    SpawnRoom(x, y + 1, itter - 1).connections[2] = true;
                    tile.connections[0] = true;
                }
            }
            if (x - 1 >= 0 && Random.Range(0f, 100f) < ((float)itter / 8f) * 100f)
            {
                if (matrix[x - 1, y] == null)
                {
                    SpawnRoom(x - 1, y, itter - 1).connections[1] = true;
                    tile.connections[3] = true;
                }
            }
            if (y - 1 >= 0 && Random.Range(0f, 100f) < ((float)itter / 8f) * 100f)
            {
                if (matrix[x, y - 1] == null)
                {
                    SpawnRoom(x, y - 1, itter - 1).connections[0] = true;
                    tile.connections[2] = true;
                }
            }

            return tile;
        }
    }

}
