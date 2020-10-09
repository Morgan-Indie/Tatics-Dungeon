using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class GridMapAdapter : MonoBehaviour
    {
        public GridMap gridMap;

        public GridCell cellPrefab;
        public GridMesh meshPrefab;
        //public GridManager gridManager;

        public EditorScript editor;
        bool loaded = false;

        GridMesh[,] gridMeshes;
        List<GridMesh> meshesToUpdate;

        private void Awake()
        {
            meshesToUpdate = new List<GridMesh>();
            gridMeshes = new GridMesh[0, 0];
        }

        private void Start()
        {
            if (editor == null)
            {
                Initalize();
            }
            else
                LoadMap();
        }

        void Initalize()
        {
            if (loaded) { return; }
            GridManager.Instance.SetMapAdapter(this);
            GridManager.Instance.SetAndLoadNewMap(gridMap);
            GridManager.Instance.SetGridState(this);
            loaded = true;
        }

        public void SetAndLoadNewMap(GridMap map)
        {
            SetMap(map);
            LoadMap();
        }

        public void SetMap(GridMap map)
        {
            foreach (GridMesh mesh in gridMeshes) { if (mesh != null) { Destroy(mesh.gameObject); } }
            gridMap = map;
        }

        public void LoadMap()
        {
            int totalY = (int)Mathf.Ceil((float)gridMap.height / (float)gridMap.maxMeshSize);
            int totalX = (int)Mathf.Ceil((float)gridMap.width / (float)gridMap.maxMeshSize);
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

            //check for new map in edit mode
            if (gridMap.mapCells.Length == 0)
                return;

            foreach (GridMesh mesh in gridMeshes) { mesh.Triangulate(); }
            ProcessMeshInteractions();
            foreach (GridMesh mesh in gridMeshes) { mesh.ApplyTriangulation(); }

            if (editor == null)
                BuildCellTemplatePrefabs();
            else
                UpdateMeshLines();
        }

        public CellState[,] GetMapState()
        {
            CellState[,] mapState = new CellState[gridMap.width, gridMap.height];
            for (int ym = 0; ym < gridMeshes.GetLength(1); ym++)
            {
                for (int xm = 0; xm < gridMeshes.GetLength(0); xm++)
                {
                    for (int y = 0; y < gridMeshes[xm, ym].cells.GetLength(1); y++)
                    {
                        for (int x = 0; x < gridMeshes[xm, ym].cells.GetLength(0); x++)
                        {
                            mapState[x + xm * gridMap.maxMeshSize, y + ym * gridMap.maxMeshSize] = gridMeshes[xm, ym].cells[x, y].GetCellState();
                        }
                    }
                }
            }
            return mapState;
        }

        void BuildCellTemplatePrefabs()
        {
            foreach (GridMesh mesh in gridMeshes)
            {
                foreach (GridCell cell in mesh.cells)
                {
                    cell.cellTemplate.BuildTemplatePrefabs(cell);
                    ProcessCellConditionals(cell);
                }
            }
        }

        void ProcessCellConditionals(GridCell cell)
        {
            if (cell.cellTemplate.stairMode != CellTemplate.StairMode.None)
            {
                //Stair position relative to its entrance
                //0 Up, 1 Right, 2 Down, 3 Left
                int dir = cell.cellTemplate.GetCellStairDirection(cell);
                IntVector2 orientation = dir % 2 == 0 ? new IntVector2(0, dir - 1) : new IntVector2((dir - 2), 0);
                if (orientation.Add(cell.gridIndex).IsValid(this)) {
                    cell.stairExits.Item1 = new IntVector2(orientation.x + cell.gridIndex.x, orientation.y + cell.gridIndex.y);
                    GetCellByIndex(orientation.Add(cell.gridIndex)).HasAdjacentStair = true;
                }
                orientation = new IntVector2(orientation.x * -1, orientation.y * -1);
                if (orientation.Add(cell.gridIndex).IsValid(this)) { 
                    cell.stairExits.Item2 = new IntVector2(orientation.x + cell.gridIndex.x, orientation.y + cell.gridIndex.y);
                    GetCellByIndex(orientation.Add(cell.gridIndex)).HasAdjacentStair = true;
                }
            }
        }

        public void CreateCells()
        {
            if (gridMap.mapCells.Length == gridMap.height)
            {
                if (gridMap.mapCells[0].height.Length != gridMap.width) { gridMap.FormatMapCells(); }
            }
            else { gridMap.FormatMapCells(); }

            for (int y = 0; y < gridMeshes.GetLength(1); y++)
            {
                for (int xs = 0; xs < gridMeshes.GetLength(0); xs++)
                {
                    int meshX = (xs + 1) * gridMap.maxMeshSize <= gridMap.width ? gridMap.maxMeshSize : gridMap.width % gridMap.maxMeshSize;
                    int meshY = (y + 1) * gridMap.maxMeshSize <= gridMap.height ? gridMap.maxMeshSize : gridMap.height % gridMap.maxMeshSize;
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
            Noise noise = new Noise();
            Vector3 position;
            position.x = x * GridMetrics.squareSize + xs * gridMap.maxMeshSize * GridMetrics.squareSize;
            position.y = 0f;
            position.z = z * GridMetrics.squareSize + y * gridMap.maxMeshSize * GridMetrics.squareSize;

            GridCell cell = cells[x, z] = Instantiate<GridCell>(cellPrefab);
            cell.transform.SetParent(gridMeshes[xs, y].transform);
            cell.transform.position = position + transform.position;
            cell.color = gridMap.mapCells[gridMap.maxMeshSize * y + z].color[xs * gridMap.maxMeshSize + x];
            cell.height = gridMap.mapCells[gridMap.maxMeshSize * y + z].height[xs * gridMap.maxMeshSize + x];
            cell.cellTemplate = gridMap.mapCells[gridMap.maxMeshSize * y + z].template[xs * gridMap.maxMeshSize + x];
            cell.SetIndices(x, z, xs, y, gridMap.maxMeshSize);
            cell.SetCellState();
        }

        public Vector3 GetPosByIndex(IntVector2 index)
        {            
            GridCell cell = GetCellByIndex(index);            
            return new Vector3(cell.transform.position.x, cell.transform.position.y, cell.transform.position.z);
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

        void UpdateMeshLines()
        {
            foreach (GridMesh mesh in gridMeshes) { mesh.RenderLines(); }
        }

        public void UpdateMeshes()
        {
            foreach (GridMesh mesh in gridMeshes) { mesh.Triangulate(); }
            ProcessMeshInteractions();
            foreach (GridMesh mesh in gridMeshes) { mesh.ApplyTriangulation(); }
            if (editor != null)
                UpdateMeshLines();
        }

        public void UpdateChangedMeshes()
        {
            foreach (GridMesh mesh in meshesToUpdate) { mesh.Triangulate(); }
            ProcessMeshInteractions();
            foreach (GridMesh mesh in meshesToUpdate) { mesh.ApplyTriangulation(); }
            meshesToUpdate.Clear();
            if (editor != null)
                UpdateMeshLines();
        }

        public void UpdateSpecificCells(Vector3 position, int range)
        {
            int x = Mathf.FloorToInt((position.x + GridMetrics.squareSize / 2) / GridMetrics.squareSize);
            int z = Mathf.FloorToInt((position.z + GridMetrics.squareSize / 2) / GridMetrics.squareSize);
            int lowerX = x - range >= 0 ? Mathf.FloorToInt((x - range) / gridMap.maxMeshSize) : 0;
            int upperX = x + range < gridMap.width ? Mathf.FloorToInt((x + range) / gridMap.maxMeshSize) : Mathf.FloorToInt((gridMap.width - 1) / gridMap.maxMeshSize);
            int lowerZ = z - range >= 0 ? Mathf.FloorToInt((z - range) / gridMap.maxMeshSize) : 0;
            int upperZ = z + range < gridMap.height ? Mathf.FloorToInt((z + range) / gridMap.maxMeshSize) : Mathf.FloorToInt((gridMap.height - 1) / gridMap.maxMeshSize);

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
            if (gridMeshes.Length == 0) { Initalize(); }
            if (index.x >= gridMap.width || index.y >= gridMap.height || index.x < 0 || index.y < 0)
            {
                //Debug.Log("Invalid Index for cell retrieval");
                return null;
            }
            try
            {
                return gridMeshes[
                        Mathf.FloorToInt((float)index.x / (float)gridMap.maxMeshSize),
                        Mathf.FloorToInt((float)index.y / (float)gridMap.maxMeshSize)
                    ].cells[
                        index.x % gridMap.maxMeshSize,
                        index.y % gridMap.maxMeshSize
                    ];
            }
            catch (System.IndexOutOfRangeException e)
            {
                Debug.Log("GetCellByIndex Called While GridMap not yet initalized");
                return null;
            }
        }

        public GridCell[,] GetNeighbors(GridCell cell)
        {
            GridCell[,] returnCells = new GridCell[3, 3];
            IntVector2 index = cell.gridIndex;
            if (index.x - 1 >= 0)
            {
                returnCells[0, 1] = GetCellByIndex(new IntVector2(index.x - 1, index.y));
                if (index.y + 1 < gridMap.height) { returnCells[0, 2] = GetCellByIndex(new IntVector2(index.x - 1, index.y + 1)); }
                if (index.y - 1 >= 0) { returnCells[0, 0] = GetCellByIndex(new IntVector2(index.x - 1, index.y - 1)); }
            }
            if (index.x + 1 < gridMap.width)
            {
                returnCells[2, 1] = GetCellByIndex(new IntVector2(index.x + 1, index.y));
                if (index.y + 1 < gridMap.height) { returnCells[2, 2] = GetCellByIndex(new IntVector2(index.x + 1, index.y + 1)); }
                if (index.y - 1 >= 0) { returnCells[2, 0] = GetCellByIndex(new IntVector2(index.x + 1, index.y - 1)); }
            }
            if (index.y + 1 < gridMap.height) { returnCells[1, 2] = GetCellByIndex(new IntVector2(index.x, index.y + 1)); }
            if (index.y - 1 >= 0) { returnCells[1, 0] = GetCellByIndex(new IntVector2(index.x, index.y - 1)); }
            return returnCells;
        }

        public GridCell[] GetOrthogonalNeighbors(GridCell cell)
        {
            IntVector2 index = cell.gridIndex;
            GridCell[] returnCells = new GridCell[4];
            if (index.y + 1 < gridMap.height) { returnCells[0] = GetCellByIndex(new IntVector2(index.x, index.y + 1)); }
            if (index.x + 1 < gridMap.width) { returnCells[1] = GetCellByIndex(new IntVector2(index.x + 1, index.y)); }
            if (index.y - 1 >= 0) { returnCells[2] = GetCellByIndex(new IntVector2(index.x, index.y - 1)); }
            if (index.x - 1 >= 0) { returnCells[3] = GetCellByIndex(new IntVector2(index.x - 1, index.y)); }
            return returnCells;
        }

        public GridCell GetCellByPos(Vector3 pos)
        {
            IntVector2 index = GetIndexByPos(pos);
            return GetCellByIndex(index);
        }

        public IntVector2 GetIndexByPos(Vector3 pos)
        {
            IntVector2 index = new IntVector2(
                    Mathf.FloorToInt(((pos.x - transform.position.x) + GridMetrics.squareSize / 2) / GridMetrics.squareSize),
                    Mathf.FloorToInt(((pos.z - transform.position.z) + GridMetrics.squareSize / 2) / GridMetrics.squareSize)
                );
            if (index.x < 0 || index.y < 0 || index.x >= gridMap.width || index.y >= gridMap.height)
                index = new IntVector2(-1, -1);

            return index;

        }

        public GridCell[] GetCellsByPosAndRange(Vector3 pos, int range)
        {
            IntVector2 index = GetIndexByPos(pos);
            return GetCellsByIndexAndRange(index, range);
        }

        public GridCell[] GetCellsByIndexAndRange(IntVector2 index, int range)
        {
            List<GridCell> returnCells = new List<GridCell>();
            IntVector2 meshIndex = new IntVector2(
                Mathf.FloorToInt((float)index.x / (float)gridMap.maxMeshSize),
                Mathf.FloorToInt((float)index.y / (float)gridMap.maxMeshSize)
                );
            IntVector2 cellIndex = new IntVector2(
                    index.x % gridMap.maxMeshSize,
                    index.y % gridMap.maxMeshSize
                );
            returnCells.Add(gridMeshes[meshIndex.x, meshIndex.y].cells[cellIndex.x, cellIndex.y]);

            int heldRange = range;
            int curX, curY;
            int maxMeshSize = gridMap.maxMeshSize;

            for (int y = 0; y < range; y++)
            {
                curY = index.y - (y + 1);
                if (curY >= 0)
                {
                    meshIndex.y = Mathf.FloorToInt(curY / maxMeshSize);
                    cellIndex.y = Mathf.FloorToInt(curY % maxMeshSize);
                    AddSpecificCell(returnCells, meshIndex, cellIndex);
                }
                curY = index.y + (y + 1);
                if (curY < gridMap.height)
                {
                    meshIndex.y = Mathf.FloorToInt(curY / maxMeshSize);
                    cellIndex.y = Mathf.FloorToInt(curY % maxMeshSize);
                    AddSpecificCell(returnCells, meshIndex, cellIndex);
                }
            }

            for (int i = 0; i < range; i++)
            {
                heldRange--;
                curX = index.x - (i + 1);
                if (curX >= 0)
                {
                    meshIndex.x = Mathf.FloorToInt(curX / maxMeshSize);
                    meshIndex.y = Mathf.FloorToInt((float)index.y / (float)gridMap.maxMeshSize);
                    cellIndex.x = Mathf.FloorToInt(curX % maxMeshSize);
                    cellIndex.y = index.y % gridMap.maxMeshSize;
                    AddSpecificCell(returnCells, meshIndex, cellIndex);
                    for (int y = 0; y < heldRange; y++)
                    {
                        curY = index.y - (y + 1);
                        if (curY >= 0)
                        {
                            meshIndex.y = Mathf.FloorToInt(curY / maxMeshSize);
                            cellIndex.y = Mathf.FloorToInt(curY % maxMeshSize);
                            AddSpecificCell(returnCells, meshIndex, cellIndex);
                        }
                        curY = index.y + (y + 1);
                        if (curY < gridMap.height)
                        {
                            meshIndex.y = Mathf.FloorToInt(curY / maxMeshSize);
                            cellIndex.y = Mathf.FloorToInt(curY % maxMeshSize);
                            AddSpecificCell(returnCells, meshIndex, cellIndex);
                        }
                    }
                }
                curX = index.x + (i + 1);
                if (curX < gridMap.width)
                {
                    meshIndex.x = Mathf.FloorToInt(curX / maxMeshSize);
                    meshIndex.y = Mathf.FloorToInt((float)index.y / (float)gridMap.maxMeshSize);
                    cellIndex.x = Mathf.FloorToInt(curX % maxMeshSize);
                    cellIndex.y = index.y % gridMap.maxMeshSize;
                    AddSpecificCell(returnCells, meshIndex, cellIndex);
                    for (int y = 0; y < heldRange; y++)
                    {
                        curY = index.y - (y + 1);
                        if (curY >= 0)
                        {
                            meshIndex.y = Mathf.FloorToInt(curY / maxMeshSize);
                            cellIndex.y = Mathf.FloorToInt(curY % maxMeshSize);
                            AddSpecificCell(returnCells, meshIndex, cellIndex);
                        }
                        curY = index.y + (y + 1);
                        if (curY < gridMap.height)
                        {
                            meshIndex.y = Mathf.FloorToInt(curY / maxMeshSize);
                            cellIndex.y = Mathf.FloorToInt(curY % maxMeshSize);
                            AddSpecificCell(returnCells, meshIndex, cellIndex);
                        }
                    }
                }
            }
            return returnCells.ToArray();
        }

        void AddSpecificCell(List<GridCell> cells, IntVector2 mesh, IntVector2 cell) { cells.Add(gridMeshes[mesh.x, mesh.y].cells[cell.x, cell.y]); }

        public GridMesh[,] GetMeshesArray() { return gridMeshes; }

        public GridCell[,] GetCellMatrix()
        {
            GridCell[,] returnValues = new GridCell[gridMap.width, gridMap.height];
            for (int ym = 0; ym < gridMeshes.GetLength(1); ym++)
            {
                for (int xm = 0; xm < gridMeshes.GetLength(0); xm++)
                {
                    for (int y = 0; y < gridMeshes[xm, ym].cells.GetLength(1); y++)
                    {
                        for (int x = 0; x < gridMeshes[xm, ym].cells.GetLength(0); x++)
                        {
                            returnValues[x + xm * gridMap.maxMeshSize, y + ym * gridMap.maxMeshSize] = gridMeshes[xm, ym].cells[x, y];
                        }
                    }
                }
            }
            return returnValues;
        }
    }
}

