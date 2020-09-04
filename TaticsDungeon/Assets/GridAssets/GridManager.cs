using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
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
                if (n.Key.GetDistance(GameManager.instance.currentCharacter.taticalMovement.currentIndex)
                    <= GameManager.instance.currentCharacter.characterStats.currentAP)
                    HighlightTileByIndex(n.Key);
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
            List<int> omits = new List<int>();
            GridCell cell;
            for (int i = 0; i < highlightedPath.Count; i++)
            {
                if (!highlightedPath[i].IsIn(indices))
                {
                    cell = mapAdapter.GetCellByIndex(highlightedPath[i]);
                    cell.RemoveHighlight();
                    cell.ApplyHighlight(validTileHighlightPrefab);
                }
                else
                {
                    //   omits.Add(highlightedPath[i].IndexInList(indices));
                }
            }
            highlightedPath.Clear();
            for (int i = 0; i < indices.Count; i++)
            {
                if (omits.Contains(i))
                    continue;
                mapAdapter.GetCellByIndex(indices[i]).ApplyHighlight(pathTileHighlightPrefab);
                highlightedPath.Add(indices[i]);
            }
        }

        public void RemoveAllHighlights()
        {
            foreach (IntVector2 index in allHighlightedTiles)
            {
                mapAdapter.GetCellByIndex(index).RemoveHighlight();
            }
            foreach (IntVector2 index in highlightedPath)
            {
                if (!index.IsIn(allHighlightedTiles))
                {
                    mapAdapter.GetCellByIndex(index).RemoveHighlight();
                }
            }
            allHighlightedTiles.Clear();
            highlightedPath.Clear();
        }

        public void RemoveCastableHighlight()
        {
            foreach (GameObject i in castableRangeHighlights)
            {
                Destroy(i);
            }
            castableRangeHighlights.Clear();
            castableHighlightOrigin.SetValues(-1, -1);
        }

        void AddCastableRangeHighlight(IntVector2 index)
        {
            castableRangeHighlights.Add(Instantiate(castableHighlightPrefab, mapAdapter.GetPosByIndex(index), Quaternion.identity));
        }

        public void HighlightCastableRange(IntVector2 index, int range)
        {
            if (!index.Equals(castableHighlightOrigin))
            {
                RemoveCastableHighlight();
                castableHighlightOrigin = index;
                
                IntVector2 checkIndex = new IntVector2(index.x, index.y);
                for (int x = 0; x <= range; x++)
                {
                    checkIndex.SetValues(index.x - x, index.y);
                    if (IndexIsOnGrid(checkIndex))
                    {
                        AddCastableRangeHighlight(checkIndex);
                        for (int y = 1; y <= range - x; y++)
                        {
                            checkIndex.SetValues(index.x - x, index.y - y);
                            if (IndexIsOnGrid(checkIndex)) { AddCastableRangeHighlight(checkIndex); }
                            checkIndex.SetValues(index.x - x, index.y + y);
                            if (IndexIsOnGrid(checkIndex)) { AddCastableRangeHighlight(checkIndex); }
                        }
                    }
                    checkIndex.SetValues(index.x + x, index.y);
                    if (IndexIsOnGrid(checkIndex) && x > 0)
                    {
                        AddCastableRangeHighlight(checkIndex);
                        for (int y = 1; y <= range - x; y++)
                        {
                            checkIndex.SetValues(index.x + x, index.y - y);
                            if (IndexIsOnGrid(checkIndex)) { AddCastableRangeHighlight(checkIndex); }
                            checkIndex.SetValues(index.x + x, index.y + y);
                            if (IndexIsOnGrid(checkIndex)) { AddCastableRangeHighlight(checkIndex); }
                        }
                    }
                }
            }
        }

        bool IndexIsOnGrid(IntVector2 index)
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

