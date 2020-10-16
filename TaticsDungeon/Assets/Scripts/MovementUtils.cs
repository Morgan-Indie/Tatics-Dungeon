using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class MovementUtils : MonoBehaviour
    {
        public static MovementUtils instance = null;

        [Header("Required")]
        public GridMapAdapter mapAdapter;
        Camera isometricCamera;
        public LayerMask meshMask;

        // Start is called before the first frame update
        private void Awake()
        {
            if (instance == null)
                instance = this;
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

        public void PrintPath(List<IntVector2> path)
        {
            foreach (IntVector2 index in path)
            {
                index.Print();
            }
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

        public Vector3 SetTargetPosition(GridCell targetCell)
        {
            Vector3 targetPos = targetCell.transform.position + targetCell.height * (GridMetrics.squareSize) * Vector3.up;

            if (targetCell.isStairs)
                targetPos += Vector3.up * .75f;

            return targetPos;
        }
    }
}

