using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PrototypeGame
{
    [CreateAssetMenu]
    public class GridMap : ScriptableObject
    {
        public int width;
        public int height;
        public int maxMeshSize;

        public MapCells[] mapCells;

        [System.Serializable]
        public class MapCells
        {
            public int[] height;
            public Color[] color;
            public CellTemplate[] template;

            public MapCells(int length)
            {
                height = new int[length];
                color = new Color[length];
                template = new CellTemplate[length];
            }
        }

        public void ProcessCells(GridCell[,] cells)
        {
            MapCells[] holdMapCells = new MapCells[cells.GetLength(1)];

            for (int y = 0; y < cells.GetLength(1); y++)
            {
                holdMapCells[y] = new MapCells(cells.GetLength(0));
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    holdMapCells[y].height[x] = cells[x, y].height;
                    holdMapCells[y].color[x] = cells[x, y].color;
                    holdMapCells[y].template[x] = cells[x, y].cellTemplate;
                }
            }

            mapCells = holdMapCells;
        }

        public void FormatMapCells()
        {
            MapCells[] holdCells = new MapCells[height];
            int xDiff = height < mapCells.Length ? mapCells.Length : height;
            int lowerY = height > mapCells.Length ? mapCells.Length : height;
            int lowerX;
            for (int y = 0; y < lowerY; y++)
            {
                lowerX = width > mapCells[y].height.Length ? mapCells[y].height.Length : width;
                holdCells[y] = new MapCells(width);
                for (int x = 0; x < width; x++)
                {
                    holdCells[y].height[x] = mapCells[y].height[x];
                    holdCells[y].color[x] = mapCells[y].color[x];
                }
                xDiff = mapCells[y].height.Length;
            }

            int yDiff = mapCells.Length;

            for (int y = 0; y < height - yDiff; y++)
            {
                holdCells[y + yDiff] = new MapCells(width);
                for (int x = 0; x < width; x++)
                {
                    holdCells[y + yDiff].height[x] = 0;
                    holdCells[y + yDiff].color[x] = Color.black;
                }
            }
            for (int y = 0; y < yDiff; y++)
            {
                for (int x = 0; x < width - xDiff; x++)
                {
                    holdCells[y].height[x + xDiff] = 0;
                    holdCells[y].color[x + xDiff] = Color.black;
                }
            }

            mapCells = holdCells;
        }
    }

}
