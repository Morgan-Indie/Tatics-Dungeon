using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class TaticalMovement : MonoBehaviour
    {
        [Header("Required")]
        public GridMapAdapter mapAdapter;

        InputHandler inputHandler;
        Transform characterTransform;
        Camera isometricCamera;
        AnimationHandler animationHandler;
        Rigidbody characterRigidBody;
        CombatUtils combatUtils;
        CharacterStats characterStats;
        Rigidbody characterRigidbody;
        CharacterStateManager characterStateManager;

        public Vector3 moveLocation=Vector3.up;

        public CharacterStateManager stateManager;

        public LayerMask characterCheckLayerMask;
        public LayerMask attackCheckLayerMask;
        public IntVector2 currentIndex;
        public GridCell currentCell;
        public IntVector2 targetIndex;
        public int currentPathIndex=0;
        public Dictionary<IntVector2, IntVector2> currentNavDict;
        public Dictionary<IntVector2, IntVector2> currentTargetsNavDict;
        public Vector3 nextPos;
        public List<IntVector2> path;
        public LayerMask meshMask;
        public IntVector2 prevIndex=new IntVector2(-1,-1);

        float movementSpeed = 3f;
        float rotationSpeed = 25f;

        // Start is called before the first frame update
        void Start()
        {
            combatUtils = GetComponent<CombatUtils>();
            stateManager = GetComponent<CharacterStateManager>();
            characterTransform = transform;
            isometricCamera = Camera.main;
            animationHandler = GetComponent<AnimationHandler>();
            characterRigidBody = GetComponent<Rigidbody>();
            characterStats = GetComponent<CharacterStats>();
            characterRigidbody = GetComponent<Rigidbody>();
            characterStateManager = GetComponent<CharacterStateManager>();

            SetCurrentCell();
        }

        public void SetCurrentIndex()
        {
            currentIndex = mapAdapter.GetIndexByPos(transform.position);
        }
        public void SetCurrentCell()
        {
            SetCurrentIndex();
            currentCell = mapAdapter.GetCellByIndex(currentIndex);
            currentCell.SetOccupyingObject(this.gameObject);

            CellState cellstate;
            if (gameObject.tag=="Player")
                cellstate = CellState.occupiedParty;
            else
                cellstate = CellState.occupiedEnemy;

            currentCell.state = cellstate;
            GridManager.Instance.gridState[currentIndex.x, currentIndex.y] = cellstate;
        }

        public void UpdateGridState()
        {
            currentCell.SetOccupyingObject(null);

            currentCell.state=CellState.open;
            //GridManager.Instance.gridState[currentIndex.x, currentIndex.y] = CellState.open;
            SetCurrentCell();
            //Debug.Log(characterStats.characterName+" Grid State Updated");
        }

        public void HandleRotation(float delta, Vector3 moveDirection)
        {
            if (moveDirection == Vector3.zero)
                moveDirection = characterTransform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            targetRotation.x = 0f;
            characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation,
                targetRotation, rotationSpeed * delta);
        }

        public IntVector2 GetMouseIndex()
        {
            RaycastHit meshHit;
            if (Input.mousePosition.y > 150f)
            {
                Ray ray = isometricCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out meshHit,meshMask))
                {
                    IntVector2 index = mapAdapter.GetIndexByPos(meshHit.point);
                    return index;
                }
            }
            return new IntVector2(-1, -1);
        }

        public int GetRequiredMoves(IntVector2 index, List<IntVector2> path)
        {
            if(path!=null)
            {
                int distance = path.Count - 1;
                return distance;
            }
            else
                return -1;
        }

        public void PrintPath(List<IntVector2> path)
        {
            foreach(IntVector2 index in path)
            {
                index.Print();
            }
        }

        public void SetCurrentNavDict()
        {
            (currentNavDict,currentTargetsNavDict) = NavigationHandler.instance.Navigate(currentIndex, characterStats.currentAP);
            if (gameObject.tag == "Player")
                GridManager.Instance.HighlightNavDict(currentNavDict);
            //Debug.Log(characterStats.characterName +" NavDict Updated");
        }

        public void SetNextPos(IntVector2 nextIndex)
        {
            GridCell nextCell = mapAdapter.GetCellByIndex(nextIndex);
            nextPos = nextCell.transform.position + nextCell.height * (GridMetrics.squareSize) * Vector3.up;
            if (nextCell.isStairs)
            {
                nextPos += Vector3.up * .75f;
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

        public void SetTargetDestination(IntVector2 targetIndex, int distance)
        {
            InputHandler.instance.tacticsXInput = false;
            GridCell cell = mapAdapter.GetCellByIndex(targetIndex);
            moveLocation = cell.transform.position;

            if (cell.isStairs)
                moveLocation += Vector3.up * .75f;

            characterStats.UseAP(distance);
            stateManager.characterAction = CharacterAction.Moving;
            stateManager.characterState = CharacterState.IsInteracting;
            characterRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            currentPathIndex = 1;

            SetNextPos(path[currentPathIndex]);
            //Debug.Log(characterStats.characterName + " Setting Destination");
        }

        public void TraverseToDestination(float delta)
        {
            if (ReachedPosition(transform.position,moveLocation))
            {
                UpdateGridState();
                characterRigidBody.velocity = Vector3.zero;
                animationHandler.PlayTargetAnimation("CombatIdle");

                animationHandler.UpdateAnimatorValues(delta, 0f);
                transform.position = moveLocation;
                currentPathIndex = 0;
                SetCurrentNavDict();

                stateManager.characterAction = CharacterAction.None;
                stateManager.characterState = CharacterState.Ready;
                characterRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                //Debug.Log(characterStats.characterName + " Reached Destination");
            }

            else
            {
                AlchemyManager.Instance.ApplyCellToPlayer(GridManager.Instance.GetCellByIndex(path[currentPathIndex]).GetComponent<CellAlchemyState>(), characterStateManager);
                if ((ReachedPosition(transform.position, nextPos)))
                {
                    currentPathIndex++;
                    SetNextPos(path[currentPathIndex]);
                }

                Vector3 currentDirection = (nextPos - transform.position);
                currentDirection.y = 0f;
                currentDirection.Normalize();
                HandleRotation(delta, currentDirection);

                characterRigidBody.velocity = movementSpeed * currentDirection;
                RaycastHit hit;
                if (Physics.Raycast(transform.position,Vector3.down,out hit,(1<<0)))
                {
                    if (hit.distance>.6 && transform.position.y>nextPos.y+.2f)
                        characterRigidBody.velocity += Vector3.down*10f;
                }
                animationHandler.UpdateAnimatorValues(delta, 1f);
            }
        }

        public void ExcuteMovement(float delta)
        {
            if (stateManager.characterAction == CharacterAction.Moving)
                TraverseToDestination(delta);
            else
            {
                IntVector2 index = GetMouseIndex();

                if (currentNavDict.ContainsKey(index))
                {
                    if (!index.Equals(prevIndex))
                        path = NavigationHandler.instance.GetPath(currentNavDict, index, currentIndex);

                    int distance = GetRequiredMoves(index,path);
                    if (characterStats.currentAP >= distance && currentNavDict.ContainsKey(index))
                    {
                        if (!index.Equals(prevIndex))
                            GridManager.Instance.HighlightPathWithList(path);
                        if ((Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput)
                            && stateManager.characterAction == CharacterAction.None)
                        {
                            SetTargetDestination(index, distance);
                        }
                    }
                }
                prevIndex = index;
            }
        }

        public void UseSkill(SkillAbstract skillScript, float delta)
        {
            skillScript.Activate(delta);
        }
    }
}
