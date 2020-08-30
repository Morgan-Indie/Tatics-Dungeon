﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Auto Filled GameObjects")]
        public PlayerMovement playerMovement;
        public Transform characterTransform;
        public TaticalMovement taticalMovement;
        public CharacterStats characterStats;
        public CharacterStateManager stateManager;
        public EnemyController enemyController;
        public bool isCurrentEnemy;
        public Skill selectedSkill;

        private void Start()
        {
            characterTransform = GetComponent<Transform>();
            taticalMovement = GetComponent<TaticalMovement>();
            playerMovement = GetComponent<PlayerMovement>();
            characterStats = GetComponent<CharacterStats>();
            stateManager = GetComponent<CharacterStateManager>();
            enemyController = GetComponent<EnemyController>();
        }

        private void FixedUpdate()
        {
            playerMovement.HandleFalling();
        }

        // Update is called once per frame
        void Update()
        {
            if (isCurrentEnemy)
            {
                float delta = Time.deltaTime;
                enemyController.FSMFixedUpdate(delta);
                if (characterStats.currentAP == 0)
                    GameManager.instance.SetNextEnemy();
            }
        }
    }
}
