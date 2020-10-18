using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class MovementUtils : MonoBehaviour
    {
        public GridMapAdapter mapAdapter;
        Camera isometricCamera;
        public LayerMask meshMask;
        public static MovementUtils Instance = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        void Start()
        {
            isometricCamera = Camera.main;
            mapAdapter = GridManager.Instance.mapAdapter;
        }

        public IntVector2 GetMouseIndex()
        {
            RaycastHit meshHit;
            if (Input.mousePosition.y > 150f)
            {
                Ray ray = isometricCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out meshHit, meshMask))
                {
                    IntVector2 index = mapAdapter.GetIndexByPos(meshHit.point);
                    return index;
                }
            }
            return new IntVector2(-1, -1);
        }

        public int GetRequiredMoves(IntVector2 index, List<IntVector2> path)
        {
            if (path != null)
            {
                int distance = path.Count - 1;
                return distance;
            }
            else
                return -1;
        }

        public bool ReachedPosition(Vector3 CP, Vector3 NP)
        {
            float dist = Mathf.Sqrt(Mathf.Pow(CP.x - NP.x, 2) + Mathf.Pow(CP.z - NP.z, 2));
            if (dist <= .2)
            {
                return true;
            }
            return false;
        }

        public void PathCellInteractions(GridCell currentCell, CharacterStateManager stateManager)
        {
            if (currentCell.InfernoTurns > 0)
                ActivateVFX.Instance.ActivateInferno(stateManager);
            else
            {
                (currentCell.substances, stateManager.characterSubstances, stateManager.statusEffects, stateManager.statusTurns,
                    currentCell.heatState.Value, stateManager.heatState.Value)
                    = AlchemyEngine.instance.CharacterCellInteractions(currentCell, stateManager);

                AlchemyEngine.instance.ApplyStateVFX(currentCell);
                AlchemyEngine.instance.ApplyStateVFX(stateManager);
            }
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

        public Vector3 GetNavPosition(GridCell cell)
        {
            Vector3 nextPos = cell.transform.position + cell.height * (GridMetrics.squareSize) * Vector3.up;
            if (cell.isStairs)
            {
                nextPos += Vector3.up * .75f;
            }
            return nextPos;
        }

        public Vector3 GetMoveDirection(Vector3 currentPos, Vector3 nextPos)
        {
            Vector3 currentDirection = (nextPos - currentPos);
            currentDirection.y = 0f;
            currentDirection.Normalize();
            return currentDirection;
        }

        public void SetToPosition(CharacterStateManager stateManager, GridCell targetCell, NavDict navDict)
        {
            PathCellInteractions(targetCell, stateManager);
            stateManager.transform.position = targetCell.transform.position;
            navDict.isDirty = true;
        }
    }
}

