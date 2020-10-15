using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PrototypeGame
{
    public class EnemyController : MonoBehaviour
    {
        public CharacterStats characterStats;
        public CharacterStateManager stateManager;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;
        public AISkillSlotHandler skillHandler;
        public SkillAbstract selectedSkill = null;
        public EnemyManager enemyManager;

        public List<GridCell> cells;
        public GridCell targetCell;
        public IntVector2 targetIndex;
        public PlayerManager target;
        public Skill skill;
        public List<IntVector2> targetPath;
        public EnemyController enemyController;
        public bool SelectedTarget = false;

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
            stateManager = GetComponent<CharacterStateManager>();
            taticalMovement = GetComponent<TaticalMovement>();
            animationHandler = GetComponent<AnimationHandler>();
            skillHandler = GetComponent<AISkillSlotHandler>();
            enemyManager = GetComponent<EnemyManager>();
        }

        public void SelectSkill()
        {
            Debug.Log("Selecting Skill");
            int choice = Random.Range(0, skillHandler.skills.Length);
            selectedSkill = skillHandler.skills[choice];
            skill = selectedSkill.skill;

            enemyManager.phase = AIActionPhase.SelectTarget;
        }

        public void MoveToTargetLocation(float delta)
        {
            if (transform.position == taticalMovement.moveLocation)
            {
                if (target != null)
                {
                    Debug.Log("Moved To Target");
                    enemyManager.phase = AIActionPhase.ExcuteSkill;
                }
            }
            else
                taticalMovement.TraverseToDestination(delta);
        }

        public void ExcuteSkill(float delta)
        {
            if (characterStats.currentAP<selectedSkill.skill.APcost)
            {
                enemyManager.phase = AIActionPhase.TurnCompleted;
            }

            else if (GameManager.instance.gameState != GameState.ResolvingInteraction)
            {
                Debug.Log("Excute Skill");                
                selectedSkill.Cast(delta, targetIndex);
            }
        }

        public void SelectTarget()
        {
            Debug.Log("Selecting Target");
            List<(PlayerManager, int)> playersHealthList = new List<(PlayerManager, int)>();
            foreach (PlayerManager player in GameManager.instance.playersDict.Values.ToList())
            {
                if (player.characterStats.currentHealth > 0)
                {
                    playersHealthList.Add((player, player.characterStats.currentHealth));
                }
            }

            playersHealthList.Sort((c1, c2) => c1.Item2.CompareTo(c2.Item2));

            //set the target destination
            for (int i = 0; i < playersHealthList.Count; i++)
            {
                target = playersHealthList[i].Item1;
                targetIndex = target.taticalMovement.currentIndex;
                targetPath = NavigationHandler.instance.GetPath(taticalMovement.currentTargetsNavDict,
                        targetIndex, taticalMovement.currentIndex);

                int totalAPCost = Mathf.Clamp(taticalMovement.GetRequiredMoves(targetIndex, targetPath) - skill.castableSettings.range,0,100) + skill.APcost;
                if (totalAPCost <= characterStats.currentAP)
                {
                    if (taticalMovement.GetRequiredMoves(targetIndex, targetPath) <= skill.castableSettings.range)
                    {
                        enemyManager.phase = AIActionPhase.ExcuteSkill;
                        return;
                    }

                    IntVector2 pathIndex = targetPath[targetPath.Count - skill.castableSettings.range - 1];
                    taticalMovement.path = targetPath;
                    taticalMovement.SetTargetDestination(pathIndex, taticalMovement.GetRequiredMoves(pathIndex, targetPath));
                    enemyManager.phase = AIActionPhase.Move;

                    return;
                }
            }

            target = playersHealthList[0].Item1;
            targetIndex = target.taticalMovement.currentIndex;
            targetPath = NavigationHandler.instance.GetPath(taticalMovement.currentTargetsNavDict,
                        targetIndex, taticalMovement.currentIndex);

            if (characterStats.currentAP+1>= taticalMovement.GetRequiredMoves(targetIndex, targetPath))
            {
                enemyManager.phase = AIActionPhase.TurnCompleted;
            }
            else
            {
                taticalMovement.path = targetPath;
                taticalMovement.SetTargetDestination(targetPath[characterStats.currentAP], characterStats.currentAP);
                enemyManager.phase = AIActionPhase.MoveToLocation;
            }
        }

        public void MoveToLocation(float delta)
        {
            if (transform.position == taticalMovement.moveLocation)
            {
                enemyManager.phase = AIActionPhase.TurnCompleted;
            }
            else
                taticalMovement.TraverseToDestination(delta);
        }
    }
}
