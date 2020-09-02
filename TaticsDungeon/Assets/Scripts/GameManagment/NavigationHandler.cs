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

            while (que.Count != 0)
            {
                que.Sort((c1, c2) => c1.Item2.CompareTo(c2.Item2));
                IntVector2 currentCell = que[0].Item1;
                que.RemoveAt(0);

                visited.Add(currentCell);

                List<IntVector2> validMoves = new List<IntVector2>()
                {
                    new IntVector2(currentCell.x+1,currentCell.y),
                    new IntVector2(currentCell.x-1,currentCell.y),
                    new IntVector2(currentCell.x,currentCell.y+1),
                    new IntVector2(currentCell.x,currentCell.y-1)
                };

                foreach (IntVector2 move in validMoves)
                {
                    //if (move.Equals(new IntVector2()));

                    if (move.IsValid(gridMapAdapter) && move.GetDistance(currentIndex) <= AP &&
                          GridManager.Instance.gridState[move.x, move.y] != CellState.obstacle && !visited.Contains(move))
                    {
                        if (GridManager.Instance.gridState[move.x, move.y] == CellState.open)
                        {
                            int altDistance = cellDistances[currentCell] + 1;
                            if (!cellDistances.ContainsKey(move) || altDistance < cellDistances[move])
                            {
                                cellDistances[move] = altDistance;
                                prevCellNav[move] = currentCell;
                                prevCellTargets[move] = currentCell;
                            }
                            if (!que.Contains((move, cellDistances[move])))
                                que.Add((move, cellDistances[move]));
                        }
                        else
                        {
                            int altDistance = cellDistances[currentCell] + 1;
                            if (!cellDistances.ContainsKey(move) || altDistance < cellDistances[move])
                            {
                                cellDistances[move] = altDistance;
                                prevCellTargets[move] = currentCell;
                            }
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
                    if (currentNavDict.ContainsKey(index))
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
