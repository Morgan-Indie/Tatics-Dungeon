using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PrototypeGame
{
    public class DamageSkill : MonoBehaviour
    {
        public List<GridCell> cells;
        public GridCell targetCell;
        public IntVector2 targetIndex;
        public PlayerManager target;
        public Skill skill;
        public TaticalMovement taticalMovement;
        public CharacterStateManager stateManager;
        public CharacterStats characterStats;
        public List<IntVector2> targetPath;
        public EnemyController enemyController;
        public bool SelectedTarget = false;

        public void Start()
        {
            taticalMovement = GetComponent<TaticalMovement>();
            stateManager = GetComponent<CharacterStateManager>();
            characterStats = GetComponent<CharacterStats>();
            enemyController = GetComponent<EnemyController>();
        }

        public void SelectTarget()
        {
            List<(PlayerManager, int)> playersHealthList = new List<(PlayerManager, int)>();
            foreach (PlayerManager player in GameManager.instance.playersDict.Values.ToList())
            {
                if (player.characterStats.currentHealth > 0)
                {
                    Debug.Log("Checking Player" + player.characterStats.characterName);
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

                int totalAPCost = taticalMovement.GetRequiredMoves(targetIndex, targetPath) - skill.castableSettings.range + skill.APcost;
                if (totalAPCost <= characterStats.currentAP)
                {
                    IntVector2 pathIndex =targetPath[targetPath.Count - skill.castableSettings.range-1];
                    taticalMovement.path = targetPath;
                    taticalMovement.SetTargetDestination(pathIndex, taticalMovement.GetRequiredMoves(pathIndex, targetPath));
                    SelectedTarget = true;
                    return;
                }
            }

            target = playersHealthList[0].Item1;
            List<IntVector2> secondaryPath = NavigationHandler.instance.GetPath(taticalMovement.currentTargetsNavDict,
                    target.taticalMovement.currentIndex, taticalMovement.currentIndex);
            taticalMovement.SetTargetDestination(secondaryPath[characterStats.currentAP - 1], characterStats.currentAP);
            target = null;
            SelectedTarget = true;
        }

        public void CastSkill(SkillAbstract skillScript, float delta)
        {
            if (SelectedTarget)
            {
                if (transform.position == taticalMovement.moveLocation && stateManager.characterAction == CharacterAction.None)
                {
                    Debug.Log("AI Target is: " + target.name);
                    if (target != null)
                        skillScript.Cast(delta, targetIndex);
                }
                else
                    taticalMovement.TraverseToDestination(delta);

            }
            else
            {
                skill = skillScript.skill;
                SelectTarget();
            }
        }
    }
}

