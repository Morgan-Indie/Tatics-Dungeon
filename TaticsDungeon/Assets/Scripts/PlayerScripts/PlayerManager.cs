using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum PlayerNumber
    {
        player1,player2,player3,player4
    }

    public class PlayerManager : MonoBehaviour
    {
        [Header("Required")]
        public GameObject playerModel;
        public PlayerNumber playerNumber;
        public CharacterClass characterClass;

        [Header("Auto Filled GameObjects")]
        public PlayerMovement playerMovement;
        public Transform playerTransform;
        public TaticalMovement taticalMovement;
        public CharacterStats characterStats;
        public CharacterStateManager stateManager;
        public InventoryHandler inventoryHandler;
        public SkillSlotsHandler skillSlotsHandler;
        public bool isCurrentPlayer;
        public Skill selectedSkill;

        private void Start()
        {
            playerTransform = GetComponent<Transform>();
            inventoryHandler = GetComponent<InventoryHandler>();
            taticalMovement = GetComponent<TaticalMovement>();
            playerMovement = GetComponent<PlayerMovement>();
            characterStats = GetComponent<CharacterStats>();
            stateManager = GetComponent<CharacterStateManager>();
            skillSlotsHandler = GetComponent<SkillSlotsHandler>();
        }

        private void FixedUpdate()
        {
            playerMovement.HandleFalling();
        }

        // Update is called once per frame
        void Update()
        {
            if (isCurrentPlayer)
            {                
                float delta = Time.deltaTime;                
                GameManager.instance.cameraModeSwitch.CheckMode();

                inventoryHandler.ActivateInventoryUI();
                InputHandler.instance.TickInput(delta);

                if (!GameManager.instance.CombatMode)
                {
                    if (stateManager.characterState == "ready")
                    {
                        playerMovement.ExcuteMovement(delta);
                        //playerMovement.HandleBlock();
                        playerMovement.HandleAttack();
                    }

                    if (GameManager.instance.playerState != "inMenu")
                        CameraHandler.instance.HandleCamera(delta);
                    else
                        playerMovement.characterRigidbody.velocity = Vector3.zero;
                }

                else
                {
                    GameManager.instance.cameraModeSwitch.isometricCamera.HandleCamera(delta);
                    if (GameManager.instance.playerState != "inMenu")
                    {
                        if (selectedSkill == null)
                            selectedSkill = skillSlotsHandler.Move;
                        taticalMovement.UseSkill(selectedSkill, delta);
                    }
                }
            }
            Debug.Log(characterStats.characterName+" Player Updated Completed");
        }
    }
}

