using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public static class CastableShapes
    {
        public static List<GridCell> GetRangeCells(Skill skill, IntVector2 index)
        {
            return CircularCells(index, skill.castableSettings.range);
        }

        public static List<GridCell> GetCastableCells(Skill skill, IntVector2 index)
        {
            switch (skill.castableSettings.shape)
            {
                case CastableShape.Single: return SingleCell(index);
                case CastableShape.Circular: return CircularCells(index, skill.castableSettings.radius);
                case CastableShape.Area: return AreaCells(index, skill.castableSettings.radius);
                case CastableShape.Line: return LineCells(index, skill.castableSettings.radius, skill.castableSettings.lineOrientation);
                case CastableShape.Cross: return CrossCells(index, skill.castableSettings.radius, skill.castableSettings.crossOrientation);
                case CastableShape.Checker: return CheckerCells(index, skill.castableSettings.radius);
            }
            return SingleCell(index);
        }

        public static List<GridCell> SingleCell(IntVector2 index)
        {
            List<GridCell> cells = new List<GridCell>();
            AddExculsiveCellToListByIndex(cells, index);
            return cells;
        }
        public static List<GridCell> CircularCells(IntVector2 index, int radius, int exclusionRadius = 0)
        {
            List<GridCell> cells = new List<GridCell>();
            IntVector2 checkIndex = new IntVector2(index.x, index.y);
            for (int x = 0; x <= radius; x++)
            {
                checkIndex.SetValues(index.x - x, index.y);
                if (GridManager.Instance.IndexIsOnGrid(checkIndex))
                {
                    if (x >= exclusionRadius)
                        AddExculsiveCellToListByIndex(cells, checkIndex);
                    for (int y = 1; y <= radius - x; y++)
                    {
                        if (y + x >= exclusionRadius)
                        {
                            AddExculsiveCellToListByIndex(cells, new IntVector2(index.x - x, index.y - y));
                            AddExculsiveCellToListByIndex(cells, new IntVector2(index.x - x, index.y + y));
                        }
                    }
                }
                checkIndex.SetValues(index.x + x, index.y);
                if (GridManager.Instance.IndexIsOnGrid(checkIndex) && x > 0)
                {
                    if (x >= exclusionRadius)
                        AddExculsiveCellToListByIndex(cells, checkIndex);
                    for (int y = 1; y <= radius - x; y++)
                    {
                        if (y + x >= exclusionRadius)
                        {
                            AddExculsiveCellToListByIndex(cells, new IntVector2(index.x + x, index.y - y));
                            AddExculsiveCellToListByIndex(cells, new IntVector2(index.x + x, index.y + y));
                        }
                    }
                }
            }

            return cells;
        }
        public static List<GridCell> AreaCells(IntVector2 index, int radius, int exclusionRadius = 0)
        {
            List<GridCell> cells = new List<GridCell>();
            int curX = 0;
            int upY = Mathf.FloorToInt((radius) / 2);
            int downY = Mathf.FloorToInt((radius + 1) / 2);
            for (int x = 0; x <= radius; x++)
            {
                for (int y = 1; y <= upY; y++)
                {
                    if (x % 2 == 0)
                        AddExculsiveCellToListByIndex(cells, new IntVector2(index.x - curX, index.y + y));
                    else
                        AddExculsiveCellToListByIndex(cells, new IntVector2(index.x + curX, index.y + y));
                }
                for (int y = 1; y <= downY; y++)
                {
                    if (x % 2 == 0)
                        AddExculsiveCellToListByIndex(cells, new IntVector2(index.x - curX, index.y - y));
                    else
                        AddExculsiveCellToListByIndex(cells, new IntVector2(index.x + curX, index.y - y));
                }
                if (x % 2 == 0)
                {
                    AddExculsiveCellToListByIndex(cells, new IntVector2(index.x - curX, index.y));
                    curX++;
                }
                else
                    AddExculsiveCellToListByIndex(cells, new IntVector2(index.x + curX, index.y));
            }

            return cells;
        }
        public static List<GridCell> LineCells(IntVector2 index, int length, int orientation, int exclusionRadius = 0)
        {
            List<GridCell> cells = new List<GridCell>();

            //0 vertical
            //1 horizontal
            IntVector2 ori = new IntVector2(orientation, Mathf.Abs(orientation - 1));
            for (int i = 0; i <= length; i++)
            {
                AddExculsiveCellToListByIndex(cells, new IntVector2(ori.x * i + index.x, ori.y * i + index.y));
                if (i != 0)
                    AddExculsiveCellToListByIndex(cells, new IntVector2(-ori.x * i + index.x, -ori.y * i + index.y));
            }

            return cells;
        }
        public static List<GridCell> CrossCells(IntVector2 index, int length, int orientation, int exclusionRadius = 0)
        {
            List<GridCell> cells = new List<GridCell>();
            IntVector2 oriA;
            IntVector2 oriB;
            if (orientation == 0)
            {
                oriA = new IntVector2(1, 0);
                oriB = new IntVector2(0, 1);
            }
            else
            {
                oriA = new IntVector2(1, 1);
                oriB = new IntVector2(-1, 1);
            }
            for (int i = 0; i <= length; i++)
            {
                AddExculsiveCellToListByIndex(cells, new IntVector2(oriA.x * i + index.x, oriA.y * i + index.y));
                if (i != 0)
                {
                    AddExculsiveCellToListByIndex(cells, new IntVector2(-oriA.x * i + index.x, -oriA.y * i + index.y));
                    AddExculsiveCellToListByIndex(cells, new IntVector2(oriB.x * i + index.x, oriB.y * i + index.y));
                    AddExculsiveCellToListByIndex(cells, new IntVector2(-oriB.x * i + index.x, -oriB.y * i + index.y));
                }
            }

            return cells;
        }
        public static List<GridCell> CheckerCells(IntVector2 index, int radius, int exclusionRadius = 0)
        {
            List<GridCell> cells = new List<GridCell>();

            int curX = 0;
            int upY = Mathf.FloorToInt((radius) / 2);
            int downY = Mathf.FloorToInt((radius + 1) / 2);
            for (int x = 0; x <= radius; x++)
            {
                for (int y = 1; y <= upY; y++)
                {
                    if ((y + curX % 2) % 2 == 1)
                    {
                        if (x % 2 == 0)
                            AddExculsiveCellToListByIndex(cells, new IntVector2(index.x - curX, index.y + y));
                        else
                            AddExculsiveCellToListByIndex(cells, new IntVector2(index.x + curX, index.y + y));
                    }

                }
                for (int y = 1; y <= downY; y++)
                {
                    if ((y + curX % 2) % 2 == 1)
                    {
                        if (x % 2 == 0)
                            AddExculsiveCellToListByIndex(cells, new IntVector2(index.x - curX, index.y - y));
                        else
                            AddExculsiveCellToListByIndex(cells, new IntVector2(index.x + curX, index.y - y));
                    }

                }

                if (x % 2 == 0)
                {
                    if (curX % 2 == 1)
                        AddExculsiveCellToListByIndex(cells, new IntVector2(index.x - curX, index.y));
                    curX++;
                }
                else if (curX % 2 == 1)
                    AddExculsiveCellToListByIndex(cells, new IntVector2(index.x + curX, index.y));

            }

            return cells;
        }

        public static void AddExculsiveCellToListByIndex(List<GridCell> cells, IntVector2 index)
        {
            if (!GridManager.Instance.IndexIsOnGrid(index))
                return;
            GridCell cell = GridManager.Instance.GetCellByIndex(index);
            if (!cells.Contains(cell)) { cells.Add(cell); }
        }
    }
}