using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    [CreateAssetMenu(fileName = "Cell Template", menuName = "New Cell Template")]
    public class CellTemplate : ScriptableObject
    {
        public string cellName = "Name";
        public Color tint = Color.white;

        public enum CellMode { SinglePrefab, PrefabList }
        public CellMode cellMode = CellMode.SinglePrefab;

        [ConditionalHide("cellMode", 0)]
        public CellDataSinglePrefab singlePrefab;
        [ConditionalHide("cellMode", 1)]
        public CellDataPrefabList prefabList;

        public enum RailingMode { None, SinglePrefab, PrefabList }
        public RailingMode railingMode = RailingMode.None;

        [ConditionalHide("railingMode", 1)]
        public RailingDataSinglePrefab railingSinglePrefab;
        [ConditionalHide("railingMode", 2)]
        public RailingDataPrefabList railingPrefabList;

        public enum ObstacleMode { None, SinglePrefab, PrefabList }
        public ObstacleMode obstacleMode = ObstacleMode.None;

        [ConditionalHide("obstacleMode", 1)]
        public ObstacleDataSinglePrefab obstacleSinglePrefab;
        [ConditionalHide("obstacleMode", 2)]
        public ObstacleDataPrefabList obstaclePrefabList;

        public enum DecorationMode { None, SinglePrefab, PrefabList }
        public DecorationMode decorationMode = DecorationMode.None;

        [ConditionalHide("decorationMode", 1)]
        public DecorationDataSinglePrefab decorationSinglePrefab;
        [ConditionalHide("decorationMode", 2)]
        public DecorationDataPrefabList decorationPrefabList;

        public enum StairMode { None, SinglePrefab, PrefabList }
        public StairMode stairMode = StairMode.None;

        [ConditionalHide("stairMode", 1)]
        public StairDataSinglePrefab stairSinglePrefab;
        [ConditionalHide("stairMode", 2)]
        public StairDataPrefabList stairPrefabList;

        public bool isAPath = false;
        [ConditionalHide("isAPath", 1)]
        public PathData pathData;

        public bool isARiver = false;
        [ConditionalHide("isARiver", 1)]
        public RiverData riverData;



        public void BuildTemplatePrefabs(GridCell cell)
        {
            GridCell[] neighborCells = GridManager.Instance.GetCellOrthogonalNeighbors(cell);
            switch (decorationMode)
            {
                case DecorationMode.SinglePrefab:
                    decorationSinglePrefab.BuildPrefab(cell);
                    break;
                case DecorationMode.PrefabList:
                    decorationPrefabList.BuildPrefab(cell);
                    break;
            }

            if (isAPath)
            {
                int[] modeDir = GetPathDirection(cell, neighborCells);
                pathData.BuildPrefab(cell, modeDir[1], modeDir[0]);
                return;
            }

            if (isARiver)
            {
                int[] modeDir = GetRiverDirection(cell, neighborCells);
                riverData.BuildPrefab(cell, modeDir[1], modeDir[0] == 1 ? true : false);
                return;
            }

            switch (cellMode)
            {
                case CellMode.SinglePrefab:
                    singlePrefab.BuildPrefab(cell);
                    break;
                case CellMode.PrefabList:
                    prefabList.BuildPrefab(cell);
                    break;
            }
            switch (railingMode)
            {
                case RailingMode.SinglePrefab:
                    for (int i = 0; i < neighborCells.Length; i++)
                    {
                        if (neighborCells[i] == null)
                            railingSinglePrefab.BuildPrefab(cell, i);
                        else if (neighborCells[i].height < cell.height)
                            railingSinglePrefab.BuildPrefab(cell, i);
                    }
                    break;
                case RailingMode.PrefabList:
                    for (int i = 0; i < neighborCells.Length; i++)
                    {
                        if (neighborCells[i] == null)
                            railingPrefabList.BuildPrefab(cell, i);
                        else if (neighborCells[i].height < cell.height)
                            railingPrefabList.BuildPrefab(cell, i);
                    }
                    break;
            }
            switch (obstacleMode)
            {
                case ObstacleMode.SinglePrefab:
                    obstacleSinglePrefab.BuildPrefab(cell);
                    break;
                case ObstacleMode.PrefabList:
                    obstaclePrefabList.BuildPrefab(cell);
                    break;
            }

            int dir;
            switch (stairMode)
            {
                case StairMode.SinglePrefab:
                    dir = GetStairDirection(cell, neighborCells);
                    if (dir < 0) { break; }
                    stairSinglePrefab.BuildPrefab(cell, dir);
                    break;
                case StairMode.PrefabList:
                    dir = GetStairDirection(cell, neighborCells);
                    if (dir < 0) { break; }
                    stairPrefabList.BuildPrefab(cell, dir);
                    break;
            }
        }

        [System.Serializable]
        public class BaseCellData
        {
            public virtual void BuildPrefab(GridCell cell)
            {
                return;
            }
        }

        [System.Serializable]
        public class CellDataSinglePrefab : BaseCellData
        {
            public GameObject prefab;
            public Vector3 offset;
            public Vector3 scale = Vector3.one;

            public override void BuildPrefab(GridCell cell)
            {
                base.BuildPrefab(cell);
                Vector3 obPos = cell.transform.position + offset + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefab, obPos, cell.transform.rotation);
                ob.transform.localScale = scale;
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class CellDataPrefabList : BaseCellData
        {
            public GameObject[] prefabs;
            public Vector3[] offsets;
            public Vector3[] scales;

            public override void BuildPrefab(GridCell cell)
            {
                base.BuildPrefab(cell);
                int rand = Random.Range(0, prefabs.Length);
                Vector3 obPos = cell.transform.position + offsets[rand] + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefabs[rand], obPos, cell.transform.rotation);
                ob.transform.localScale = scales[rand];
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class RailingDataSinglePrefab
        {
            public GameObject prefab;
            public Vector3 positiveOffset;
            public Vector3 scale = Vector3.one;
            //up, right, down, left
            Vector3[] directions = { new Vector3(0, 1, 1), new Vector3(1, 1, 0), new Vector3(0, 1, -1), new Vector3(-1, 1, 0) };

            public void BuildPrefab(GridCell cell, int dir)
            {
                Vector3 obPos = cell.transform.position + Vector3.Scale(positiveOffset, directions[dir]) + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefab, obPos, cell.transform.rotation);
                ob.transform.localScale = scale;
                ob.transform.Rotate(new Vector3(0, 90 * dir, 0));
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class RailingDataPrefabList
        {
            public GameObject[] prefabs;
            public Vector3[] positiveOffsets;
            public Vector3[] scales;
            //up, right, down, left
            Vector3[] directions = { new Vector3(0, 1, 1), new Vector3(1, 1, 0), new Vector3(0, 1, -1), new Vector3(-1, 1, 0) };

            public void BuildPrefab(GridCell cell, int dir)
            {
                int rand = Random.Range(0, prefabs.Length);
                Vector3 obPos = cell.transform.position + Vector3.Scale(positiveOffsets[rand], directions[dir]) + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefabs[rand], obPos, cell.transform.rotation);
                ob.transform.localScale = scales[rand];
                ob.transform.Rotate(new Vector3(0, 90 * dir, 0));
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class ObstacleDataSinglePrefab
        {
            public float percentToSpawn = 100f;
            public bool placeRandomly = false;
            [ConditionalHide("placeRandomly", 1)]
            public Vector3 randomLimit;
            public GameObject prefab;
            public Vector3 offset;
            public Vector3 scale = Vector3.one;
            public void BuildPrefab(GridCell cell)
            {
                if (Random.Range(0f, 100f) > percentToSpawn) { return; }
                Vector3 obPos = cell.transform.position + offset + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefab, obPos, cell.transform.rotation);
                ob.transform.localScale = scale;
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class ObstacleDataPrefabList
        {
            public float percentToSpawn = 100f;
            public bool placeRandomly = false;
            [ConditionalHide("placeRandomly", 1)]
            public Vector3 randomLimit;
            public GameObject[] prefabs;
            public Vector3[] offsets;
            public Vector3[] scales;
            public void BuildPrefab(GridCell cell)
            {
                if (Random.Range(0f, 100f) > percentToSpawn) { return; }
                int rand = Random.Range(0, prefabs.Length);
                Vector3 obPos = cell.transform.position + offsets[rand] + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefabs[rand], obPos, cell.transform.rotation);
                ob.transform.localScale = scales[rand];
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class DecorationDataSinglePrefab
        {
            public float percentToSpawn = 100f;
            public bool placeRandomly = false;
            [ConditionalHide("placeRandomly", 1)]
            public Vector3 randomLimit = Vector3.zero;
            public GameObject prefab;
            public Vector3 offset;
            public Vector3 scale = Vector3.one;
            public void BuildPrefab(GridCell cell)
            {
                if (Random.Range(0f, 100f) > percentToSpawn) { return; }
                Vector3 randOffset = new Vector3(Random.Range(0f, randomLimit.x * 2) - randomLimit.x, Random.Range(0f, randomLimit.y * 2) - randomLimit.y, Random.Range(0f, randomLimit.z * 2) - randomLimit.z);
                Vector3 obPos = cell.transform.position + offset + randOffset + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefab, obPos, cell.transform.rotation);
                ob.transform.localScale = scale;
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class DecorationDataPrefabList
        {
            public float percentToSpawn = 100f;
            public bool placeRandomly = false;
            [ConditionalHide("placeRandomly", 1)]
            public Vector3 randomLimit = Vector3.zero;
            public GameObject[] prefabs;
            public Vector3[] offsets;
            public Vector3[] scales;
            public void BuildPrefab(GridCell cell)
            {
                if (Random.Range(0f, 100f) > percentToSpawn) { return; }
                int rand = Random.Range(0, prefabs.Length);
                Vector3 randOffset = new Vector3(Random.Range(0f, randomLimit.x * 2) - randomLimit.x, Random.Range(0f, randomLimit.y * 2) - randomLimit.y, Random.Range(0f, randomLimit.z * 2) - randomLimit.z);
                Vector3 obPos = cell.transform.position + offsets[rand] + randOffset + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefabs[rand], obPos, cell.transform.rotation);
                ob.transform.localScale = scales[rand];
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class StairDataSinglePrefab
        {
            public GameObject prefab;
            public Vector3 offset;
            public Vector3 scale = Vector3.one;
            public float rotationOffset = 0f;
            //up, right, down, left
            Vector3[] directions = { new Vector3(0, 1, 1), new Vector3(1, 1, 0), new Vector3(0, 1, -1), new Vector3(-1, 1, 0) };

            public void BuildPrefab(GridCell cell, int dir)
            {
                Vector3 obPos = cell.transform.position + Vector3.Scale(offset, directions[dir]) + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefab, obPos, cell.transform.rotation);
                ob.transform.localScale = scale;
                ob.transform.Rotate(new Vector3(0, 90 * dir + rotationOffset, 0));
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class StairDataPrefabList
        {
            public GameObject[] prefabs;
            public Vector3[] offsets;
            public Vector3[] scales;
            public float rotationOffset = 0f;
            //up, right, down, left
            Vector3[] directions = { new Vector3(0, 1, 1), new Vector3(1, 1, 0), new Vector3(0, 1, -1), new Vector3(-1, 1, 0) };

            public void BuildPrefab(GridCell cell, int dir)
            {
                int rand = Random.Range(0, prefabs.Length);
                Vector3 obPos = cell.transform.position + Vector3.Scale(offsets[rand], directions[dir]) + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefabs[rand], obPos, cell.transform.rotation);
                ob.transform.localScale = scales[rand];
                ob.transform.Rotate(new Vector3(0, 90 * dir + rotationOffset, 0));
                ob.transform.SetParent(cell.transform);
            }
        }

        [System.Serializable]
        public class PathData
        {
            public GameObject straightPath;
            public GameObject cornerPath;
            public GameObject tPath;
            public GameObject crossPath;
            public GameObject bridge;
            public GameObject riverUnderBridge;

            public Vector3[] offsets;
            public Vector3[] scales;
            public float[] rotationOffsets;

            public void BuildPrefab(GridCell cell, int dir, int mode)
            {
                GameObject prefab = straightPath;
                switch (mode)
                {
                    case 1: prefab = cornerPath; break;
                    case 2: prefab = tPath; break;
                    case 3: prefab = crossPath; break;
                    case 4: prefab = bridge; break;
                }
                Vector3[] pathDir = { new Vector3(-1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, -1), new Vector3(-1, 1, -1) };
                //swap the x and z of the offset for ever second 90 degree rotation
                Vector3 processedOffset = dir % 2 == 0 ? offsets[mode] : new Vector3(offsets[mode].z, offsets[mode].y, offsets[mode].x);
                Vector3 obPos = cell.transform.position + Vector3.Scale(pathDir[dir], processedOffset) + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefab, obPos, cell.transform.rotation);
                ob.transform.localScale = scales[mode];
                ob.transform.Rotate(new Vector3(0, 90 * dir + rotationOffsets[mode], 0));
                ob.transform.SetParent(cell.transform);

                if (mode == 4)
                {
                    mode++;
                    dir = dir + 1 < 4 ? dir + 1 : dir - 3;
                    obPos = cell.transform.position + Vector3.Scale(pathDir[dir], offsets[mode]) + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                    ob = Instantiate(riverUnderBridge, obPos, cell.transform.rotation);
                    ob.transform.localScale = scales[mode];
                    ob.transform.Rotate(new Vector3(0, 90 * dir + rotationOffsets[mode], 0));
                    ob.transform.SetParent(cell.transform);
                }
            }
        }

        [System.Serializable]
        public class RiverData
        {
            public GameObject straightPrefab;
            public GameObject cornerPrefab;

            public Vector3[] offsets;
            public Vector3[] scales;

            public float rotationOffset = 0f;

            public void BuildPrefab(GridCell cell, int dir, bool isCorner)
            {
                Vector3[] riverDir = { new Vector3(-1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, -1), new Vector3(-1, 1, -1) };
                GameObject prefab = isCorner ? cornerPrefab : straightPrefab;
                int index = isCorner ? 1 : 0;
                Vector3 obPos = cell.transform.position + Vector3.Scale(riverDir[dir], offsets[index]) + new Vector3(0, cell.height * GridMetrics.heightIncrement, 0);
                GameObject ob = Instantiate(prefab, obPos, cell.transform.rotation);
                ob.transform.localScale = scales[index];
                ob.transform.Rotate(new Vector3(0, 90 * dir + rotationOffset, 0));
                ob.transform.SetParent(cell.transform);
            }
        }
        //checks first for stairs, then for pathes, giving priority to stair tiles
        public int GetStairDirection(GridCell cell, GridCell[] neighborCells)
        {
            for (int i = 0; i < neighborCells.Length; i++)
            {
                if (neighborCells[i] == null)
                    continue;
                if (neighborCells[i].cellTemplate.stairMode != StairMode.None)
                {
                    if (neighborCells[i].height > cell.height)
                        return i;
                    if (neighborCells[i].height < cell.height)
                        return i + 2 < 4 ? i + 2 : i - 2;
                }
            }
            for (int i = 0; i < neighborCells.Length; i++)
            {
                if (neighborCells[i] == null)
                    continue;
                if (neighborCells[i].cellTemplate.isAPath)
                {
                    if (neighborCells[i].height > cell.height)
                        return i;
                    if (neighborCells[i].height < cell.height)
                        return i + 2 < 4 ? i + 2 : i - 2;
                }
            }
            return -1;
        }

        // mode / dir
        //0 - straight
        //1 - corner
        //2 - T
        //3 - cross
        //4 - bridge
        public int[] GetPathDirection(GridCell cell, GridCell[] neighborCells)
        {
            int totalPaths = 0; int totalRivers = 0; int lastRiverIndex = -1;
            bool[] paths = new bool[4];
            int[] returnValues = { 0, 0 };

            for (int i = 0; i < neighborCells.Length; i++)
            {
                if (neighborCells[i] == null)
                    continue;

                if (neighborCells[i].cellTemplate.isAPath || neighborCells[i].cellTemplate.stairMode != StairMode.None)
                {
                    totalPaths++;
                    paths[i] = true;
                }
                if (neighborCells[i].cellTemplate.isARiver)
                {
                    totalRivers++;
                    lastRiverIndex = i;
                }
            }
            if (totalRivers == 2)
            {
                returnValues[0] = 4;
                returnValues[1] = lastRiverIndex;
            }
            else if (totalPaths == 4)
                returnValues[0] = 3;
            else if (totalPaths == 3)
            {
                returnValues[0] = 2;
                for (int i = 0; i < paths.Length; i++)
                {
                    if (!paths[i])
                    {
                        returnValues[1] = i + 2 < 4 ? i + 2 : i - 2;
                        break;
                    }
                }
            }
            else if (totalPaths == 2)
            {
                for (int i = 0; i < paths.Length; i++)
                {
                    if (paths[i])
                    {
                        returnValues[1] = i;
                        returnValues[0] = 0;
                        int checkInt = i + 1 < 4 ? i + 1 : i - 3;
                        //check counterclockwise if index is zero
                        int othercheck = i == 0 ? 3 : checkInt;
                        if (paths[checkInt] || paths[othercheck])
                        {
                            returnValues[0] = 1;
                            if (!paths[checkInt])
                                returnValues[1] = checkInt + 2 < 4 ? checkInt + 2 : checkInt - 2;
                        }
                        break;
                    }
                }
            }
            else if (totalPaths == 1)
            {
                returnValues[0] = 0;
                for (int i = 0; i < paths.Length; i++) { if (paths[i]) { returnValues[1] = i; } }
            }

            return returnValues;
        }

        public int[] GetRiverDirection(GridCell cell, GridCell[] neighborCells)
        {
            int totalSegments = 0;
            bool[] rivers = new bool[4];
            int[] returnValues = { 0, 0 };
            for (int i = 0; i < neighborCells.Length; i++)
            {
                if (neighborCells[i] == null)
                    continue;
                if (neighborCells[i].cellTemplate.isARiver)
                {
                    totalSegments++;
                    rivers[i] = true;
                }
            }
            if (totalSegments == 1)
            {
                returnValues[0] = 0;
                for (int i = 0; i < rivers.Length; i++)
                {
                    if (rivers[i]) { returnValues[1] = i; }
                }
            }
            else
            {
                for (int i = 0; i < rivers.Length; i++)
                {
                    if (rivers[i])
                    {
                        returnValues[1] = i;
                        //check if river is straight
                        int checkInt = i + 2 < 4 ? i + 2 : i - 2;
                        if (!rivers[checkInt])
                        {
                            returnValues[0] = 1;
                            //if not, check for corner direction
                            checkInt = i + 1 < 4 ? i + 1 : i - 3;
                            if (!rivers[checkInt])
                                returnValues[1] = checkInt + 2 < 4 ? checkInt + 2 : checkInt - 2;
                        }
                        break;
                    }
                }
            }
            return returnValues;
        }
    }

}
