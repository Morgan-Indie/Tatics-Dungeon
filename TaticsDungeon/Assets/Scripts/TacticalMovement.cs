using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class TacticalMovement : MonoBehaviour
    {
        [Header("Required")]
        public GridMapAdapter mapAdapter;
        public NavDict navDict;
        public CharacterLocation location;
        public LocalMotion localMotion;

        public Vector3 moveLocation;
        public CharacterStateManager stateManager;

        public IntVector2 targetIndex;
        public GridCell targetCell;
        public int currentPathIndex;

        public Vector3 nextPos;
        public List<IntVector2> path;

        // Start is called before the first frame update
        void Start()
        {
            stateManager = GetComponent<CharacterStateManager>();
            location = GetComponent<CharacterLocation>();
            navDict = GetComponent<NavDict>();
            mapAdapter = GridManager.Instance.mapAdapter;
        }

        public void SetNextPos()
        {
            nextPos = MovementUtils.Instance.GetNavPosition(mapAdapter.GetCellByIndex(path[currentPathIndex]));
            currentPathIndex++;
        }

        public void SetPath()
        {
            path = MovementUtils.Instance.GetPath(navDict.currentNavDict, targetIndex, location.currentIndex);
        }

        public void SetTargetDestination(IntVector2 index)
        {
            targetIndex = index;
            targetCell = mapAdapter.GetCellByIndex(index);

            currentPathIndex = 1;
            SetNextPos();
        }

        public void TraverseToDestination(float delta)
        { 
            if (MovementUtils.Instance.ReachedPosition(transform.position, moveLocation))
            {
                MovementUtils.Instance.SetToPosition(stateManager, location.currentCell, navDict);

                stateManager.characterAction = CharacterAction.None;
                stateManager.characterState = CharacterState.Ready;
            }

            else
            {
                if (MovementUtils.Instance.ReachedPosition(transform.position, nextPos))
                {
                    MovementUtils.Instance.SetToPosition(stateManager, location.currentCell, navDict);
                    SetNextPos();
                }

                localMotion.Move(MovementUtils.Instance.GetMoveDirection(transform.position, nextPos), nextPos, delta);
            }
        }
    }
}

