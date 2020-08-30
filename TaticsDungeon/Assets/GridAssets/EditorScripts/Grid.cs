using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PrototypeGame
{
    public class Grid : MonoBehaviour
    {
        public int width = 6;
        public int height = 6;
        public int maxMeshSize = 50;
        public float smoothing = 20f;

        public Color[] colors;

        public GridCell cellPrefab;
        public EditorScript editor;

        public GridMesh meshPrefab;

        Canvas gridCanvas;
        GridMesh[,] gridMeshes;
        List<GridMesh> meshesToUpdate;

        private void Awake()
        {
            meshesToUpdate = new List<GridMesh>();

            gridCanvas = GetComponentInChildren<Canvas>();

            int totalY = (int)Mathf.Ceil((float)height / (float)maxMeshSize);
            int totalX = (int)Mathf.Ceil((float)width / (float)maxMeshSize);
            gridMeshes = new GridMesh[totalX, totalY];
            for (int y = 0; y < totalY; y++)
            {
                for (int x = 0; x < totalX; x++)
                {
                    gridMeshes[x, y] = Instantiate(meshPrefab);
                    gridMeshes[x, y].transform.SetParent(transform);
                }
            }
            CreateCells();
        }

        private void Start()
        {
            foreach (GridMesh mesh in gridMeshes)
            {
                mesh.Triangulate();
            }
            ProcessMeshInteractions();
            foreach (GridMesh mesh in gridMeshes)
            {
                mesh.ApplyTriangulation();
            }
        }

        public void CreateCells()
        {
            for (int y = 0; y < gridMeshes.GetLength(1); y++)
            {
                for (int xs = 0; xs < gridMeshes.GetLength(0); xs++)
                {
                    int meshX = (xs + 1) * maxMeshSize <= width ? maxMeshSize : width % maxMeshSize;
                    int meshY = (y + 1) * maxMeshSize <= height ? maxMeshSize : height % maxMeshSize;
                    gridMeshes[xs, y].cells = new GridCell[meshX, meshY];

                    for (int z = 0; z < meshY; z++)
                    {
                        for (int x = 0; x < meshX; x++)
                        {
                            CreateCell(x, z, xs, y, gridMeshes[xs, y].cells);
                        }
                    }
                }
            }
        }

        void CreateCell(int x, int z, int xs, int y, GridCell[,] cells)
        {
            Vector3 position;
            position.x = x * GridMetrics.squareSize + xs * maxMeshSize * GridMetrics.squareSize;
            position.y = 0f;
            position.z = z * GridMetrics.squareSize + y * maxMeshSize * GridMetrics.squareSize;

            GridCell cell = cells[x, z] = Instantiate<GridCell>(cellPrefab);
            cell.transform.SetParent(gridMeshes[xs, y].transform);
            cell.transform.position = position + transform.position;
            cell.color = colors[(int)Random.Range(0, colors.Length)];
            cell.height = 0;
            cell.SetIndices(x, z, xs, y, maxMeshSize);
        }

        void ProcessMeshInteractions()
        {
            GridCell[,] zeroCell = new GridCell[1, 1];
            CreateCell(0, 0, 0, 0, zeroCell);
            zeroCell[0, 0].height = -10; zeroCell[0, 0].color = Color.black;
            for (int z = 0; z < gridMeshes.GetLength(1); z++)
            {
                for (int x = 0; x < gridMeshes.GetLength(0); x++)
                {
                    for (int xs = 0; xs < gridMeshes[x, z].cells.GetLength(0); xs++)
                    {
                        if (z - 1 < 0)
                        {
                            zeroCell[0, 0].transform.position = new Vector3(gridMeshes[x, z].cells[xs, 0].transform.position.x, zeroCell[0, 0].height, gridMeshes[x, z].cells[xs, 0].transform.position.z - GridMetrics.squareSize);
                            gridMeshes[x, z].DrawVerticalCellInteraction(zeroCell[0, 0], gridMeshes[x, z].cells[xs, 0]);
                        }
                        if (z + 1 >= gridMeshes.GetLength(1))
                        {
                            zeroCell[0, 0].transform.position = new Vector3(gridMeshes[x, z].cells[xs, gridMeshes[x, z].cells.GetLength(1) - 1].transform.position.x, zeroCell[0, 0].height, gridMeshes[x, z].cells[xs, gridMeshes[x, z].cells.GetLength(1) - 1].transform.position.z + GridMetrics.squareSize);
                            gridMeshes[x, z].DrawVerticalCellInteraction(gridMeshes[x, z].cells[xs, gridMeshes[x, z].cells.GetLength(1) - 1], zeroCell[0, 0]);
                        }
                        else
                        {
                            gridMeshes[x, z].DrawVerticalCellInteraction(gridMeshes[x, z].cells[xs, gridMeshes[x, z].cells.GetLength(1) - 1], gridMeshes[x, z + 1].cells[xs, 0]);
                        }
                    }
                    for (int zs = 0; zs < gridMeshes[x, z].cells.GetLength(1); zs++)
                    {
                        if (x - 1 < 0)
                        {
                            zeroCell[0, 0].transform.position = new Vector3(gridMeshes[x, z].cells[0, zs].transform.position.x - GridMetrics.squareSize, zeroCell[0, 0].height, gridMeshes[x, z].cells[0, zs].transform.position.z);
                            gridMeshes[x, z].DrawHorizontalCellInteraction(zeroCell[0, 0], gridMeshes[x, z].cells[0, zs]);
                        }
                        if (x + 1 >= gridMeshes.GetLength(0))
                        {
                            zeroCell[0, 0].transform.position = new Vector3(gridMeshes[x, z].cells[gridMeshes[x, z].cells.GetLength(0) - 1, zs].transform.position.x + GridMetrics.squareSize, zeroCell[0, 0].height, gridMeshes[x, z].cells[gridMeshes[x, z].cells.GetLength(0) - 1, zs].transform.position.z);
                            gridMeshes[x, z].DrawHorizontalCellInteraction(gridMeshes[x, z].cells[gridMeshes[x, z].cells.GetLength(0) - 1, zs], zeroCell[0, 0]);
                        }
                        else
                        {
                            gridMeshes[x, z].DrawHorizontalCellInteraction(gridMeshes[x, z].cells[gridMeshes[x, z].cells.GetLength(0) - 1, zs], gridMeshes[x + 1, z].cells[0, zs]);
                        }
                    }
                }
            }
        }

        public void ReBuildMesh(int w, int h, float s)
        {
            width = w;
            height = h;
            smoothing = s;
            CreateCells();
            foreach (GridMesh mesh in gridMeshes) { mesh.Triangulate(); }
        }

        public void UpdateMeshes()
        {
            foreach (GridMesh mesh in gridMeshes)
            {
                mesh.Triangulate();
            }
            ProcessMeshInteractions();
            foreach (GridMesh mesh in gridMeshes)
            {
                mesh.ApplyTriangulation();
            }
        }

        public void UpdateChangedMeshes()
        {
            foreach (GridMesh mesh in meshesToUpdate)
            {
                mesh.Triangulate();
            }
            ProcessMeshInteractions();
            foreach (GridMesh mesh in meshesToUpdate)
            {
                mesh.ApplyTriangulation();
            }
            meshesToUpdate.Clear();
        }

        public void UpdateSpecificCells(Vector3 position, int range)
        {
            int x = Mathf.FloorToInt((position.x + GridMetrics.squareSize / 2) / GridMetrics.squareSize);
            int z = Mathf.FloorToInt((position.z + GridMetrics.squareSize / 2) / GridMetrics.squareSize);
            int lowerX = x - range >= 0 ? Mathf.FloorToInt((x - range) / maxMeshSize) : 0;
            int upperX = x + range < width ? Mathf.FloorToInt((x + range) / maxMeshSize) : Mathf.FloorToInt((width - 1) / maxMeshSize);
            int lowerZ = z - range >= 0 ? Mathf.FloorToInt((z - range) / maxMeshSize) : 0;
            int upperZ = z + range < height ? Mathf.FloorToInt((z + range) / maxMeshSize) : Mathf.FloorToInt((height - 1) / maxMeshSize);

            for (int zs = 0; zs < gridMeshes.GetLength(1); zs++)
            {
                for (int xs = 0; xs < gridMeshes.GetLength(0); xs++)
                {
                    if (xs <= upperX && xs >= lowerX && zs <= upperZ && zs >= lowerZ)
                    {
                        meshesToUpdate.Add(gridMeshes[xs, zs]);
                    }
                }
            }
            UpdateChangedMeshes();
        }

        public GridCell GetCellByIndex(IntVector2 index)
        {
            if (index.x >= width || index.y >= height || index.x < 0 || index.y < 0)
            {
                Debug.Log("Invalid Index for cell retrieval");
                return null;
            }
            return gridMeshes[
                    Mathf.FloorToInt((float)index.x / (float)maxMeshSize),
                    Mathf.FloorToInt((float)index.y / (float)maxMeshSize)
                ].cells[
                    index.x % maxMeshSize,
                    index.y % maxMeshSize
                ];
        }

        public GridCell[,] GetNeighbors(GridCell cell)
        {
            GridCell[,] returnCells = new GridCell[3, 3];
            IntVector2 index = cell.gridIndex;
            if (index.x - 1 >= 0)
            {
                returnCells[0, 1] = GetCellByIndex(new IntVector2(index.x - 1, index.y));
                if (index.y + 1 < height) { returnCells[0, 2] = GetCellByIndex(new IntVector2(index.x - 1, index.y + 1)); }
                if (index.y - 1 >= 0) { returnCells[0, 0] = GetCellByIndex(new IntVector2(index.x - 1, index.y - 1)); }
            }
            if (index.x + 1 < width)
            {
                returnCells[2, 1] = GetCellByIndex(new IntVector2(index.x + 1, index.y));
                if (index.y + 1 < height) { returnCells[2, 2] = GetCellByIndex(new IntVector2(index.x + 1, index.y + 1)); }
                if (index.y - 1 >= 0) { returnCells[2, 0] = GetCellByIndex(new IntVector2(index.x + 1, index.y - 1)); }
            }
            if (index.y + 1 < height) { returnCells[1, 2] = GetCellByIndex(new IntVector2(index.x, index.y + 1)); }
            if (index.y - 1 >= 0) { returnCells[1, 0] = GetCellByIndex(new IntVector2(index.x, index.y - 1)); }
            return returnCells;
        }

        public GridCell GetCellByPos(Vector3 position)
        {
            int x = (int)Mathf.Floor((position.x + GridMetrics.squareSize / 2) / GridMetrics.squareSize);
            int z = (int)Mathf.Floor((position.z + GridMetrics.squareSize / 2) / GridMetrics.squareSize);
            int meshX = Mathf.FloorToInt(x / maxMeshSize);
            int meshZ = Mathf.FloorToInt(z / maxMeshSize);
            int cellX = Mathf.FloorToInt(x % maxMeshSize);
            int cellZ = Mathf.FloorToInt(z % maxMeshSize);
            return gridMeshes[meshX, meshZ].cells[cellX, cellZ];
        }

        public GridCell[] GetCellsByPosAndRange(Vector3 position, int range)
        {
            List<GridCell> returnCells = new List<GridCell>();
            int x = (int)Mathf.Floor((position.x + GridMetrics.squareSize / 2) / GridMetrics.squareSize);
            int z = (int)Mathf.Floor((position.z + GridMetrics.squareSize / 2) / GridMetrics.squareSize);
            int meshX = Mathf.FloorToInt(x / maxMeshSize);
            int meshZ = Mathf.FloorToInt(z / maxMeshSize);
            int cellX = Mathf.FloorToInt(x % maxMeshSize);
            int cellZ = Mathf.FloorToInt(z % maxMeshSize);
            returnCells.Add(gridMeshes[meshX, meshZ].cells[cellX, cellZ]);

            int heldRange = range;
            for (int i = 0; i < range - 1; i++)
            {
                heldRange--;
                int curX = x - (i + 1);
                if (curX >= 0)
                {
                    int curMeshX = Mathf.FloorToInt(curX / maxMeshSize);
                    int curCellX = Mathf.FloorToInt(curX % maxMeshSize);
                    returnCells.Add(gridMeshes[curMeshX, meshZ].cells[curCellX, cellZ]);
                    for (int y = 0; y < heldRange - 1; y++)
                    {
                        int curZ = z - (y + 1);
                        if (curZ >= 0) { returnCells.Add(gridMeshes[curMeshX, Mathf.FloorToInt(curZ / maxMeshSize)].cells[curCellX, Mathf.FloorToInt(curZ % maxMeshSize)]); }
                        curZ = z + (y + 1);
                        if (curZ < height) { returnCells.Add(gridMeshes[curMeshX, Mathf.FloorToInt(curZ / maxMeshSize)].cells[curCellX, Mathf.FloorToInt(curZ % maxMeshSize)]); }
                    }
                }
                curX = x + (i + 1);
                if (curX < width)
                {
                    int curMeshX = Mathf.FloorToInt(curX / maxMeshSize);
                    int curCellX = Mathf.FloorToInt(curX % maxMeshSize);
                    returnCells.Add(gridMeshes[curMeshX, meshZ].cells[curCellX, cellZ]);
                    for (int y = 0; y < heldRange - 1; y++)
                    {
                        int curZ = z - (y + 1);
                        if (curZ >= 0) { returnCells.Add(gridMeshes[curMeshX, Mathf.FloorToInt(curZ / maxMeshSize)].cells[curCellX, Mathf.FloorToInt(curZ % maxMeshSize)]); }
                        curZ = z + (y + 1);
                        if (curZ < height) { returnCells.Add(gridMeshes[curMeshX, Mathf.FloorToInt(curZ / maxMeshSize)].cells[curCellX, Mathf.FloorToInt(curZ % maxMeshSize)]); }
                    }
                }
            }

            for (int y = 0; y < range - 1; y++)
            {
                int curZ = z - (y + 1);
                if (curZ >= 0) { returnCells.Add(gridMeshes[meshX, Mathf.FloorToInt(curZ / maxMeshSize)].cells[cellX, Mathf.FloorToInt(curZ % maxMeshSize)]); }
                curZ = z + (y + 1);
                if (curZ < height) { returnCells.Add(gridMeshes[meshX, Mathf.FloorToInt(curZ / maxMeshSize)].cells[cellX, Mathf.FloorToInt(curZ % maxMeshSize)]); }
            }

            return returnCells.ToArray();
        }


        public GridMesh[,] GetMeshesArray() { return gridMeshes; }
    }

}
