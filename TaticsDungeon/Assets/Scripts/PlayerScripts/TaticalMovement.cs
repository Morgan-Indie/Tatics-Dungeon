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

        public Vector3 moveLocation=Vector3.up;

        public LayerMask characterCheckLayerMask;
        public LayerMask attackCheckLayerMask;
        public GameObject navDummy;
        public IntVector2 currentIndex;
        public GridCell currentCell;
        public IntVector2 targetIndex;
        public int currentPathIndex=0;
        public Dictionary<IntVector2, IntVector2> currentNavDict;
        public Vector3 nextPos;
        public List<IntVector2> path;
        public LayerMask meshMask;

        float movementSpeed = 3.5f;
        float rotationSpeed = 5f;

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
                    //IntVector2 index = mapAdapter.GetIndexByPos(mapAdapter.transform.InverseTransformPoint(ray.GetPoint(rayDistance)));
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

        public int GetMouseDistance(IntVector2 index)
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
            currentNavDict = NavigationHandler.instance.Navigate(currentIndex, characterStats.currentAP);
            if (gameObject.tag == "Player")
                GridManager.Instance.HighlightNavDict(currentNavDict);
            Debug.Log(characterStats.characterName +" NavDict Updated");
        }

        public void SetTargetDestination(IntVector2 targetIndex, int distance)
        {
            InputHandler.instance.tacticsXInput = false;
            GridCell cell = mapAdapter.GetCellByIndex(targetIndex);
            moveLocation = cell.transform.position;
            
            characterStats.UseAP(distance);
            stateManager.characterAction = CharacterAction.Moving;
            currentPathIndex = 0;
            
            nextPos = mapAdapter.GetCellByIndex(path[currentPathIndex]).transform.position;
            Debug.Log(characterStats.characterName + " Setting Destination");    
        }

        public void TraverseToDestination(float delta)
        {
            if ((nextPos - transform.position).magnitude < .1)
            {
                if ((transform.position - moveLocation).magnitude <= .1)
                {
                    stateManager.characterAction = CharacterAction.None;
                                       
                    UpdateGridState();
                    characterRigidBody.velocity = Vector3.zero;
                                        
                    animationHandler.UpdateAnimatorValues(delta, 0f);   
                    transform.position = moveLocation;
                    currentPathIndex = 0;
                    
                    if (characterStats.currentAP > 0)
                        SetCurrentNavDict();
                    else
                    {
                        GridManager.Instance.RemoveAllHighlights();
                    }
                    Debug.Log(characterStats.characterName + " Reached Destination"); 
                }
                else
                {
                    currentPathIndex++;
                    nextPos = mapAdapter.GetCellByIndex(path[currentPathIndex]).transform.position;
                }
            }

            else
            {
                Vector3 currentDirection = (nextPos - transform.position).normalized;
                HandleRotation(delta, currentDirection);
                characterRigidBody.velocity = currentDirection * movementSpeed;
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
                    int distance = GetMouseDistance(index);
                    
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

