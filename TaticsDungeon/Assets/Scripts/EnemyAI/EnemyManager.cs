using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum AIActionPhase
    {
        SelectSkill, Move, ExcuteSkill, SwitchTurn, SelectTarget, CheckCurrentAP, MoveToLocation, TurnCompleted
    }

    public class EnemyManager : MonoBehaviour
    {
        [Header("Auto Filled GameObjects")]
        public Transform characterTransform;
        public TaticalMovement taticalMovement;
        public CharacterStats characterStats;
        public CharacterStateManager stateManager;
        public EnemyController enemyController;
        public AISkillSlotHandler skillSlotHandler;
        public AIActionPhase phase = AIActionPhase.SelectSkill;
            
        public bool isCurrentEnemy;

        private void Start()
        {
            characterTransform = GetComponent<Transform>();
            taticalMovement = GetComponent<TaticalMovement>();
            characterStats = GetComponent<CharacterStats>();
            stateManager = GetComponent<CharacterStateManager>();
            enemyController = GetComponent<EnemyController>();
            skillSlotHandler = GetComponent<AISkillSlotHandler>();
        }

        public void DisableCharacter()
        {
            GameManager.instance.enemiesDict.Remove(characterStats.characterName);
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        public void EnemyUpdate(float delta)
        {            
            if (isCurrentEnemy)
            {        
                if (stateManager.characterState == CharacterState.Disabled)
                {
                    phase = AIActionPhase.TurnCompleted;
                }

                switch(phase)
                {
                    case AIActionPhase.SelectSkill:
                        enemyController.SelectSkill();
                        break;

                    case AIActionPhase.SelectTarget:
                        enemyController.SelectTarget();
                        break;

                    case AIActionPhase.Move:
                        enemyController.MoveToTargetLocation(delta);
                        break;

                    case AIActionPhase.ExcuteSkill:
                        enemyController.ExcuteSkill(delta);
                        break;

                    case AIActionPhase.CheckCurrentAP:
                        if (characterStats.currentAP <= 0)
                        {
                            phase = AIActionPhase.TurnCompleted;
                        }

                        else
                            phase = AIActionPhase.SelectSkill;
                        break;

                    case AIActionPhase.TurnCompleted:
                        GameManager.instance.SetNextEnemy();
                        break;

                    case AIActionPhase.MoveToLocation:
                        enemyController.MoveToLocation(delta);
                        break;
                }
            }
        }
    }
}

