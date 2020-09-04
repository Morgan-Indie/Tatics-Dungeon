using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrototypeGame
{
    public class NavigationHandler : MonoBehaviour
    {
        [Header("Required")]
        public GridMapAdapter gridMapAdapter;
        public static NavigationHandler instance = null;

        public void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public (Dictionary<IntVector2, IntVector2>, Dictionary<IntVector2, IntVector2>) Navigate(IntVector2 currentIndex, int AP)
        {
            Dictionary<IntVector2, int> cellDistances = new Dictionary<IntVector2, int>() { { currentIndex, 0 } };
            Dictionary<IntVector2, IntVector2> prevCellNav = new Dictionary<IntVector2, IntVector2>() { };
            Dictionary<IntVector2, IntVector2> prevCellTargets = new Dictionary<IntVector2, IntVector2>() { };
            List<IntVector2> visited = new List<IntVector2>();

            List<(IntVector2, int)> que = new List<(IntVector2, int)>() { (currentIndex, 0) };

            void UpdateNavDicts(IntVector2 currentCellIndex, IntVector2 nextCellIndex, bool targetDictOnly = false)
            {
                int altDistance = cellDistances[currentCellIndex] + 1;
                if (!cellDistances.ContainsKey(nextCellIndex) || altDistance < cellDistances[nextCellIndex])
                {
                    cellDistances[nextCellIndex] = altDistance;
                    prevCellTargets[nextCellIndex] = currentCellIndex;
                    if (!targetDictOnly)
                        prevCellNav[nextCellIndex] = currentCellIndex;

                }
                if (!targetDictOnly)
                {
                    if (!que.Contains((nextCellIndex, cellDistances[nextCellIndex])))
                        que.Add((nextCellIndex, cellDistances[nextCellIndex]));
                }
            }

            while (que.Count != 0)
            {
                que.Sort((c1, c2) => c1.Item2.CompareTo(c2.Item2));
                IntVector2 currentCellIndex = que[0].Item1;
                que.RemoveAt(0);

                visited.Add(currentCellIndex);

                List<IntVector2> validMoves = new List<IntVector2>()
                {
                    new IntVector2(currentCellIndex.x+1,currentCellIndex.y),
                    new IntVector2(currentCellIndex.x-1,currentCellIndex.y),
                    new IntVector2(currentCellIndex.x,currentCellIndex.y+1),
                    new IntVector2(currentCellIndex.x,currentCellIndex.y-1)
                };

                foreach (IntVector2 nextCellIndex in validMoves)
                {
                    if (nextCellIndex.IsValid(gridMapAdapter) && nextCellIndex.GetDistance(currentIndex) <= AP &&
                          gridMapAdapter.GetCellByIndex(nextCellIndex).state != CellState.obstacle && !visited.Contains(nextCellIndex))
                    {
                        GridCell currentCell = gridMapAdapter.GetCellByIndex(currentCellIndex);
                        GridCell nextCell = gridMapAdapter.GetCellByIndex(nextCellIndex);

                        if (nextCell.state == CellState.open)
                        {
                            Debug.Log(nextCellIndex);
                            if (nextCell.isStairs)
                            {
                                if (currentCellIndex.Equals(nextCell.stairExits.Item1) || currentCellIndex.Equals(nextCell.stairExits.Item2))
                                    UpdateNavDicts(currentCellIndex, nextCellIndex);
                            }

                            else if (currentCell.isStairs)
                            {
                                if (currentCell.stairExits.Item1.Equals(nextCellIndex) || currentCell.stairExits.Item2.Equals(nextCellIndex))
                                    UpdateNavDicts(currentCellIndex, nextCellIndex);
                            }

                            else if (currentCell.height == nextCell.height)
                                UpdateNavDicts(currentCellIndex, nextCellIndex);
                            
                        }
                        else
                        {
                            if (nextCell.isStairs)
                            {
                                if (currentCellIndex.Equals(nextCell.stairExits.Item1) || currentCellIndex.Equals(nextCell.stairExits.Item2))
                                    UpdateNavDicts(currentCellIndex, nextCellIndex,true);
                            }

                            else if (currentCell.isStairs)
                            {
                                if (currentCell.stairExits.Item1.Equals(nextCellIndex) || currentCell.stairExits.Item2.Equals(nextCellIndex))
                                    UpdateNavDicts(currentCellIndex, nextCellIndex,true);
                            }

                            else if (currentCell.height == nextCell.height)
                                UpdateNavDicts(currentCellIndex, nextCellIndex,true);
                        }
                    }
                }
            }
            return (prevCellNav, prevCellTargets);
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
