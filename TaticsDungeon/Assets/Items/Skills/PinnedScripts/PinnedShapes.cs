using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum PinnedShape
    {
        Single,
        Line, 
        Circle,
        Cone,

    }

    public static class PinnedShapes
    {
        public static List<GridCell> GetPinnedCells(Skill skill, IntVector2  origin, IntVector2 index)
        {
            switch (skill.pinnedSettings.shape)
            {
                case PinnedShape.Single: return SingleCell(origin, index);
                case PinnedShape.Circle:
                    int exclude = skill.pinnedSettings.inclusive ? 0 : 1;
                    return CircularCells(skill, origin, exclude);
                case PinnedShape.Line: return LineCells(skill, origin, index);
                case PinnedShape.Cone: return ConeCells(skill, origin, index);
            }
            return SingleCell(origin, index);
        }

        public static List<GridCell> CircularCells(Skill skill, IntVector2 index, int exclusionRadius=0)
        {
            List<GridCell> cells = new List<GridCell>();
            int radius = skill.pinnedSettings.radius;
            if (skill.pinnedSettings.shape == PinnedShape.Cone)
                radius *= 2;
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
        public static List<GridCell> SingleCell(IntVector2 origin, IntVector2 index)
        {
            List<GridCell> cells = new List<GridCell>();

            int dir = origin.GetSection(index, 4);
            int x = dir < 3 ? 1 - dir : 0;
            int y = dir < 2 ? -dir : dir - 2;

            AddExculsiveCellToListByIndex(cells, new IntVector2(x + origin.x, y + origin.y));

            return cells;
        }
        public static List<GridCell> LineCells(Skill skill, IntVector2 origin, IntVector2 index)
        {
            List<GridCell> cells = new List<GridCell>();
            int dir = origin.GetSection(index, 4);
            int x = dir < 3 ? 1 - dir : 0;
            int y = dir < 2 ? -dir : dir - 2;
            if (skill.pinnedSettings.inclusive)
                AddExculsiveCellToListByIndex(cells, origin);
            
            IntVector2 checkIndex = new IntVector2(origin.x, origin.y);
            IntVector2 increment = new IntVector2(x, y);
            for (int i=0; i<skill.pinnedSettings.radius; i++)
            {
                checkIndex = checkIndex + increment;
                AddExculsiveCellToListByIndex(cells, checkIndex);
            }

            return cells;
        }
        public static List<GridCell> ConeCells(Skill skill, IntVector2 origin, IntVector2 index)
        {
            List<GridCell> cells = new List<GridCell>();
            int dir = origin.GetSection(index, 4);
            int x = dir < 3 ? 1 - dir : 0;
            int y = dir < 2 ? -dir : dir - 2;
            if (skill.pinnedSettings.inclusive)
                AddExculsiveCellToListByIndex(cells, origin);
            IntVector2 checkIndex = new IntVector2(origin.x, origin.y);
            IntVector2 increment = new IntVector2(x, y);
            IntVector2 inverseIncrement = new IntVector2(Mathf.Abs(y), Mathf.Abs(x));
            int expansion = 0;
            for (int i=0; i<skill.pinnedSettings.radius; i++)
            {
                checkIndex = checkIndex + increment;
                AddExculsiveCellToListByIndex(cells, checkIndex);
                if ((i+1) % skill.pinnedSettings.coneSettings.expansion == 0)
                    expansion++;
                for (int n=1; n<=expansion; n++)
                {
                    AddExculsiveCellToListByIndex(
                        cells,
                        new IntVector2(checkIndex.x + n * inverseIncrement.x, checkIndex.y + n * inverseIncrement.y)
                    );
                    AddExculsiveCellToListByIndex(
                        cells,
                        new IntVector2(checkIndex.x - n * inverseIncrement.x, checkIndex.y - n * inverseIncrement.y)
                    );
                }
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