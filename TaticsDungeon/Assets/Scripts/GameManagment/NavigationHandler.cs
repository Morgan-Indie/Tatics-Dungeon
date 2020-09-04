using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrototypeGame
{
    public enum CellState { open, obstacle, occupiedParty, occupiedEnemy };

    class NavigationHandler:MonoBehaviour
    {
        [Header("Required")]
        public GridMapAdapter gridMapAdapter;
        public static NavigationHandler instance = null;

        public void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public Dictionary<IntVector2, IntVector2> Navigate(IntVector2 currentIndex,int AP)
        {
            Dictionary<IntVector2, int> cellDistances = new Dictionary<IntVector2, int>() { { currentIndex, 0 } };
            Dictionary<IntVector2, IntVector2> prevCell = new Dictionary<IntVector2, IntVector2>() { };
            List<IntVector2> visited = new List<IntVector2>();

            List<(IntVector2, int)> que = new List<(IntVector2, int)>() { (currentIndex, 0) };
            
<<<<<<< Updated upstream
=======
            void UpdatePaths(IntVector2 currentCellIndex, IntVector2 nextCellIndex, bool targetsOnly = false)
            {
                int altDistance = cellDistances[currentCellIndex] + 1;
                if (!cellDistances.ContainsKey(nextCellIndex) || altDistance < cellDistances[nextCellIndex])
                {
                    cellDistances[nextCellIndex] = altDistance;
                    prevCellTargets[nextCellIndex] = currentCellIndex;

                    if (!targetsOnly)
                        prevCellNav[nextCellIndex] = currentCellIndex;
                }
                if (!que.Contains((nextCellIndex, cellDistances[nextCellIndex])))
                    que.Add((nextCellIndex, cellDistances[nextCellIndex]));
            }

>>>>>>> Stashed changes
            while (que.Count != 0)
            {                
                que.Sort((c1, c2) => c1.Item2.CompareTo(c2.Item2));
                IntVector2 currentCellIndex = que[0].Item1;
                que.RemoveAt(0);

                visited.Add(currentCellIndex);

                List<IntVector2> validMoves = new List<IntVector2>()
                {
<<<<<<< Updated upstream
                    new IntVector2(currentCell.x+1,currentCell.y),
                    new IntVector2(currentCell.x-1,currentCell.y),
                    new IntVector2(currentCell.x,currentCell.y+1),
                    new IntVector2(currentCell.x,currentCell.y-1)
                };                
=======
                    new IntVector2(currentCellIndex.x+1,currentCellIndex.y),
                    new IntVector2(currentCellIndex.x-1,currentCellIndex.y),
                    new IntVector2(currentCellIndex.x,currentCellIndex.y+1),
                    new IntVector2(currentCellIndex.x,currentCellIndex.y-1)
                };
>>>>>>> Stashed changes

                foreach (IntVector2 nextCellIndex in validMoves)
                {
<<<<<<< Updated upstream
                    //if (move.Equals(new IntVector2()));

                    if (move.IsValid(gridMapAdapter) && move.GetDistance(currentIndex) <= AP &&
                          GridManager.Instance.gridState[move.x,move.y]==CellState.open && !visited.Contains(move))
                    {                        
                        int altDistance = cellDistances[currentCell] + 1;
                        if (!cellDistances.ContainsKey(move) || altDistance < cellDistances[move])
                        {
                            cellDistances[move] = altDistance;
                            prevCell[move] = currentCell;
=======
                    if (nextCellIndex.IsValid(gridMapAdapter) && nextCellIndex.GetDistance(currentIndex) <= AP &&
                          gridMapAdapter.GetCellByIndex(nextCellIndex).state != CellState.obstacle && !visited.Contains(nextCellIndex))
                    {
                        GridCell nextCell = gridMapAdapter.GetCellByIndex(nextCellIndex);
                        GridCell currentCell = gridMapAdapter.GetCellByIndex(currentCellIndex);
                        // if cellstate is open both navdicts will collect the paths.
                        if (GridManager.Instance.gridState[nextCellIndex.x, nextCellIndex.y] == CellState.open)
                        {
                            // you can only enter stairs if you are included in stair exits tuple
                            if (nextCell.isStairs)
                            {
                                if (nextCell.stairExits.Item1.Equals(currentCellIndex) ||
                                    nextCell.stairExits.Item2.Equals(currentCellIndex))
                                {
                                    UpdatePaths(currentCellIndex, nextCellIndex);
                                }
                            }

                            // you can only transition between heights if you're on a stairs tile and the next tile is included in your exits.
                            else if (nextCell.height!=currentCell.height)
                            {
                                if (currentCell.isStairs)
                                {
                                    if (currentCell.stairExits.Item1.Equals(nextCellIndex) || currentCell.stairExits.Item2.Equals(nextCellIndex))
                                        UpdatePaths(currentCellIndex, nextCellIndex);
                                }
                            }

                            // normal tile transitions without height changes or stairs.
                            else
                            {
                                UpdatePaths(currentCellIndex,nextCellIndex);
                            }
                        }

                        // only target nav dict for enemy AI will collect paths to interesting targets.
                        else
                        {
                            if (nextCell.isStairs)
                            {
                                if (nextCell.stairExits.Item1.Equals(currentCellIndex) ||
                                    nextCell.stairExits.Item2.Equals(currentCellIndex))
                                {
                                    UpdatePaths(currentCellIndex, nextCellIndex,true);
                                }
                            }

                            else if (nextCell.height != currentCell.height)
                            {
                                if (currentCell.isStairs)
                                {
                                    if (currentCell.stairExits.Item1.Equals(nextCellIndex) || currentCell.stairExits.Item2.Equals(nextCellIndex))
                                        UpdatePaths(currentCellIndex, nextCellIndex, true);
                                }
                            }

                            else
                            {
                                UpdatePaths(currentCellIndex, nextCellIndex, true);
                            }
>>>>>>> Stashed changes
                        }
                        if (!que.Contains((move, cellDistances[move])))
                            que.Add((move, cellDistances[move]));
                    }
                }
            }
            return prevCell;
        }

        public List<IntVector2> GetPath(Dictionary<IntVector2, IntVector2> currentNavDict, 
            IntVector2 targetIndex, IntVector2 currentIndex)
        {
            List<IntVector2> path = new List<IntVector2>();
            
            if (currentNavDict.ContainsKey(targetIndex))
            {
                IntVector2 index = targetIndex;
                while (!index.Equals(currentIndex))
                {   
                    path.Add(index);
                    index = currentNavDict[index];
                }
                path.Add(currentIndex);
                path.Reverse();
                return path;
            }
            else
                return null;
        }
    }
}
