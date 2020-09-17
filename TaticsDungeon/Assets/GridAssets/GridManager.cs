using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PrototypeGame
{
    public enum CellHighlightType
    {
        None,
        Walkable,
        Invalid,
        Path,
        InRange,
        Castable,
    }

    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        //  public GameObject unitPrefab;
        //  public GridMapAdapter gridMapAdapter;

        //  List<GridCell> highlightedCells;
        //  UnitScript activeUnit;
        //  int team1units = 3;
        //  int team2units = 3;

        GridMapAdapter mapAdapter;
        public GameObject roomTranistionPrefab;
        List<GameObject> roomTransitions;

        public GameObject gridHolder;
        public GameObject tileHighlightPrefab;

        [Header("Tile Highlights")]
        public GameObject validTileHighlightPrefab;
        public GameObject inValidTileHighlightPrefab;
        public GameObject pathTileHighlightPrefab;

        [HideInInspector]
        public List<IntVector2> highlightedPath;
        [HideInInspector]
        public List<IntVector2> allHighlightedTiles;
        public List<GameObject> castableRangeHighlights;
        IntVector2 castableHighlightOrigin = new IntVector2(-1, -1);
        public GameObject castableHighlightPrefab;
        public GameObject castableValidPrefab;

        public CellState[,] gridState;

        private void Awake()
        {
            roomTransitions = new List<GameObject>();
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void SetGridState(GridMapAdapter mapAdapter) { gridState = mapAdapter.GetMapState(); }
        public void LoadMap() { mapAdapter.LoadMap(); UpdateGridLineHighlights(); }
        public void SetAndLoadNewMap(GridMap map) { mapAdapter.SetAndLoadNewMap(map); UpdateGridLineHighlights(); }

        public void TranistionRoom(IntVector2 index)
        {
            FloorManager.Instance.AddToRoomIndex(index.x, index.y);
            foreach (GameObject i in roomTransitions)
            {
                if (i != null)
                    Destroy(i);
            }
            roomTransitions.Clear();
            SetAndLoadNewMap(FloorManager.Instance.GetCurrentMap());
            AddTransitions();
            FloorManager.Instance.MovePlayerMarker();
        }

        public void AddTransitions()
        {
            FloorTile tile = FloorManager.Instance.GetCurrentFloorTile();
            float subtractor = -1.5f;
            float offset = GridMetrics.squareSize * 15 - subtractor;
            for (int i = 0; i < tile.connections.Length; i++)
            {
                if (tile.connections[i])
                {
                    GameObject ob = Instantiate(roomTranistionPrefab, Vector3.zero, Quaternion.identity);
                    roomTransitions.Add(ob);
                    ob.GetComponentInChildren<RoomConnection>().direction = new IntVector2((int)tile.connectionMults[i].x, (int)tile.connectionMults[i].y);
                    ob.transform.position = new Vector3(tile.connectionMults[i].x * offset + subtractor, 0, tile.connectionMults[i].y * offset + subtractor);
                    if (i % 2 != 0)
                        ob.transform.Rotate(0, 90, 0);
                }
            }
        }

        public void SetMapAdapter(GridMapAdapter m) { mapAdapter = m; }
        public GridCell[,] GetCellNeighbors(GridCell cell) { return mapAdapter.GetNeighbors(cell); }
        public GridCell[] GetCellOrthogonalNeighbors(GridCell cell) { return mapAdapter.GetOrthogonalNeighbors(cell); }
        public GridCell GetCellByIndex(IntVector2 index) { return mapAdapter.GetCellByIndex(index); }


        void UpdateGridLineHighlights()
        {
            if (gridHolder == null)
                return;
            Transform[] previousHighlights = gridHolder.GetComponentsInChildren<Transform>();
            foreach (Transform child in previousHighlights) { if (child != gridHolder.transform) { Destroy(child.gameObject); } }
            GridCell[,] cells = mapAdapter.GetCellMatrix();
            foreach (GridCell cell in cells)
            {
                GameObject ob = Instantiate(tileHighlightPrefab, cell.transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
                ob.transform.SetParent(gridHolder.transform);
            }
        }

        public void HighlightNavDict(Dictionary<IntVector2, IntVector2> dict)
        {
            RemoveAllHighlights();
            foreach (KeyValuePair<IntVector2, IntVector2> n in dict)
            {
                HighlightTileByIndex(n.Key);
                allHighlightedTiles.Add(n.Key);
            }
        }

        public void HighlightTileByIndex(IntVector2 index)
        {
            if (!index.IsIn(allHighlightedTiles)) { allHighlightedTiles.Add(index); }
            GridCell cell = mapAdapter.GetCellByIndex(index);
            CellState currentState = cell.GetCellState();
            if (currentState == CellState.open)
                cell.ApplyHighlight(validTileHighlightPrefab);
            else
                cell.ApplyHighlight(inValidTileHighlightPrefab);
        }

        public void HighlightPathWithList(List<IntVector2> indices)
        {
            foreach (IntVector2 index in highlightedPath.Except(indices))
            {
                GridCell cell = mapAdapter.GetCellByIndex(index);
                cell.RemoveHighlight();
                cell.ApplyHighlight(validTileHighlightPrefab);
            }

            foreach (IntVector2 index in indices.Except(highlightedPath))
            {
                GridCell cell = mapAdapter.GetCellByIndex(index);
                cell.RemoveHighlight();
                cell.ApplyHighlight(pathTileHighlightPrefab);
            }

            highlightedPath = indices;
        }

        public void RemoveAllHighlights()
        {           
            foreach(var item in allHighlightedTiles.Union(highlightedPath))
            {
                mapAdapter.GetCellByIndex(item).RemoveHighlight();
            }
            allHighlightedTiles.Clear();
            highlightedPath.Clear();
        }

        public void HighlightCastableRange(IntVector2 playerOrigin, IntVector2 castOrigin, Skill skill)
        {

            List<GridCell> rangeCells = new List<GridCell>();
            List<GridCell> castRange = new List<GridCell>();
            List<GridCell> outerRange = new List<GridCell>();
            switch (skill.castType)
            {
                case CastType.Free:
                    rangeCells = CastableShapes.GetRangeCells(skill, playerOrigin);
                    castRange = CastableShapes.GetCastableCells(skill, castOrigin);
                    outerRange = CastableShapes.CircularCells(playerOrigin, skill.castableSettings.radius + skill.castableSettings.range, skill.castableSettings.range + 1);
                    break;
                case CastType.Pinned:
                    rangeCells = new List<GridCell>();
                    castRange = PinnedShapes.GetPinnedCells(skill, playerOrigin, castOrigin);
                    outerRange = PinnedShapes.CircularCells(skill, playerOrigin);
                    break;
            }
            foreach (GridCell cell in outerRange)
            {
                  if (!castRange.Contains(cell))
                     cell.RemoveHighlight();
            }

            foreach (GridCell cell in rangeCells)
            {
                cell.ApplyHighlight(castableValidPrefab, CellHighlightType.InRange);
                allHighlightedTiles.Add(cell.index);
            }
            if (castOrigin.GetDistance(playerOrigin) <= skill.castableSettings.range)
            {
                foreach (GridCell cell in castRange)
                {
                    cell.ApplyHighlight(castableHighlightPrefab, CellHighlightType.Castable);
                    allHighlightedTiles.Add(cell.index);
                }
            }
            
        }

        public bool IndexIsOnGrid(IntVector2 index)
        {
            if (index.x >= 0 &&
                index.y >= 0 &&
                index.x < mapAdapter.gridMap.width &&
                index.y < mapAdapter.gridMap.height)
            {
                return true;
            }
            return false;
        }

        public List<GridCell> GetCellsByIndexAndRange(IntVector2 index, int range)
        {
            List<GridCell> cells = new List<GridCell>();
            IntVector2 checkIndex = new IntVector2(index.x, index.y);
            for (int x = 0; x <= range; x++)
            {
                checkIndex.SetValues(index.x - x, index.y);
                if (IndexIsOnGrid(checkIndex))
                {
                    AddExculsiveCellToListByIndex(cells, checkIndex);
                    for (int y = 1; y <= range - x; y++)
                    {
                        checkIndex.SetValues(index.x - x, index.y - y);
                        if (IndexIsOnGrid(checkIndex)) { AddExculsiveCellToListByIndex(cells, checkIndex); }
                        checkIndex.SetValues(index.x - x, index.y + y);
                        if (IndexIsOnGrid(checkIndex)) { AddExculsiveCellToListByIndex(cells, checkIndex); }
                    }
                }
                checkIndex.SetValues(index.x + x, index.y);
                if (IndexIsOnGrid(checkIndex) && x > 0)
                {
                    AddExculsiveCellToListByIndex(cells, checkIndex);
                    for (int y = 1; y <= range - x; y++)
                    {
                        checkIndex.SetValues(index.x + x, index.y - y);
                        if (IndexIsOnGrid(checkIndex)) { AddExculsiveCellToListByIndex(cells, checkIndex); }
                        checkIndex.SetValues(index.x + x, index.y + y);
                        if (IndexIsOnGrid(checkIndex)) { AddExculsiveCellToListByIndex(cells, checkIndex); }
                    }
                }
            }
            return cells;
        }

        void AddExculsiveCellToListByIndex(List<GridCell> cells, IntVector2 index)
        {
            GridCell cell = mapAdapter.GetCellByIndex(index);
            if (!cells.Contains(cell))  { cells.Add(cell); } 
        }
    }
}

