using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class TaticalMovement : MonoBehaviour
    {
        [Header("Required")]
        public GridMapAdapter mapAdapter;
        public Collider triggerCollider;

        InputHandler inputHandler;
        Transform characterTransform;
        Camera isometricCamera;
        AnimationHandler animationHandler;
        Rigidbody characterRigidBody;
        CharacterStateManager stateManager;
        CombatUtils combatUtils;
        CharacterStats characterStats;
        Rigidbody characterRigidbody;

        public Vector3 moveLocation=Vector3.up;

        public LayerMask characterCheckLayerMask;
        public LayerMask attackCheckLayerMask;
        public GameObject navDummy;
        public IntVector2 currentIndex;
        public GridCell currentCell;
        public IntVector2 targetIndex;
        public int currentPathIndex=0;
        public Dictionary<IntVector2, IntVector2> currentNavDict;
        public Dictionary<IntVector2, IntVector2> currentTargetsNavDict;
        public Vector3 nextPos;
        public List<IntVector2> path;
        public LayerMask meshMask;
        public bool isDirty;

        float movementSpeed = 4f;
        float rotationSpeed = 15f;

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

            SetCurrentCell();
            triggerCollider.enabled = false;
        }

        public void SetCurrentIndex()
        {
            currentIndex = mapAdapter.GetIndexByPos(transform.position);
        }
        public void SetCurrentCell()
        {                           
            SetCurrentIndex();
            currentCell = mapAdapter.GetCellByIndex(currentIndex);
            currentCell.SetCharacter(this.gameObject);

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
            currentCell.SetCharacter(null);
            
            currentCell.state=CellState.open;
            GridManager.Instance.gridState[currentIndex.x, currentIndex.y] = CellState.open;
            SetCurrentCell();
            Debug.Log(characterStats.characterName+" Grid State Updated");
        }

        public void HandleRotation(float delta, Vector3 moveDirection)
        {
            if (moveDirection == Vector3.zero)
                moveDirection = characterTransform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
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

        public GameObject EnemyCheck(IntVector2 index)
        {
            GridCell cell = mapAdapter.GetCellByIndex(index);
            Vector3 targetLocation = cell.transform.position;
            RaycastHit hit;

            Vector3 rayCastLoction = targetLocation + Vector3.down * 2;
            if (Physics.Raycast(rayCastLoction, Vector3.up, out hit, Mathf.Infinity, attackCheckLayerMask))
            {
                return hit.collider.gameObject;
            }
            return null;
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
            Debug.Log(characterStats.characterName +" NavDict Updated");
        }

        public void SetNextPos(IntVector2 nextIndex)
        {
            GridCell nextCell = mapAdapter.GetCellByIndex(nextIndex);
            nextPos = nextCell.transform.position + nextCell.height * GridMetrics.squareSize * Vector3.up;
            if (nextCell.isStairs)
            {
                nextPos += Vector3.up * .75f;
            }
        }

        private bool ReachedPosition(Vector3 CP, Vector3 NP)
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
            Debug.Log(characterStats.characterName + " Setting Destination");    
        }

        public void TraverseToDestination(float delta)
        {
            if (ReachedPosition(transform.position,moveLocation))
            {
                UpdateGridState();
                characterRigidBody.velocity = Vector3.zero;

                animationHandler.UpdateAnimatorValues(delta, 0f);
                transform.position = moveLocation;
                currentPathIndex = 0;
                SetCurrentNavDict();

                stateManager.characterAction = CharacterAction.None;
                stateManager.characterState = CharacterState.Ready;
                characterRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                Debug.Log(characterStats.characterName + " Reached Destination");
            }

            else if ((ReachedPosition(transform.position, nextPos)))
            {
                currentPathIndex++;
                SetNextPos(path[currentPathIndex]);
            }

            else
            {
                Vector3 currentDirection = (nextPos - transform.position);
                currentDirection.y = 0f;
                currentDirection.Normalize();
                HandleRotation(delta, currentDirection);
                characterRigidBody.velocity = currentDirection * movementSpeed;
                RaycastHit hit;
                if (Physics.Raycast(transform.position,Vector3.down,out hit))
                {
                    if (hit.distance >.35 && (transform.position.y-nextPos.y)>.2)
                        characterRigidBody.velocity += (Vector3.down * 500f * delta);
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
                    path = NavigationHandler.instance.GetPath(currentNavDict, index, currentIndex);
                    int distance = GetRequiredMoves(index,path);
                    
                    if (characterStats.currentAP >= distance && EnemyCheck(index) == null)
                    {
                        GridManager.Instance.HighlightPathWithList(path);
                        if ((Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput) 
                            && stateManager.characterAction == CharacterAction.None)
                        {                           
                            SetTargetDestination(index, distance);                            
                        }
                    }
                }
            }
        }

        public void UseSkill(Skill skill, float delta)
        {
            SkillFactory.instance.Activate(characterStats, animationHandler, this, skill, delta);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
                stateManager.skillColliderTiggered = true;
        }
    }
}

