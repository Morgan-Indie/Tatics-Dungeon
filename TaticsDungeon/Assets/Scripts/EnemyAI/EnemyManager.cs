﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Auto Filled GameObjects")]
        public Transform characterTransform;
        public TaticalMovement taticalMovement;
        public CharacterStats characterStats;
        public CharacterStateManager stateManager;
        public EnemyController enemyController;
        public bool isCurrentEnemy;

        private void Start()
        {
            characterTransform = GetComponent<Transform>();
            taticalMovement = GetComponent<TaticalMovement>();
            characterStats = GetComponent<CharacterStats>();
            stateManager = GetComponent<CharacterStateManager>();
            enemyController = GetComponent<EnemyController>();
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
                if (characterStats.currentAP == 0 && GameManager.instance.gameState == GameState.Ready)
                    GameManager.instance.SetNextEnemy();
                else
                { 
                    if (enemyController.selectedSkill == null)
                    {
                        enemyController.SelectSkill(); 
                    }
                    enemyController.Act(delta);
                }
            }
        }
    }
}

