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
            characterStats = GetComponent<CharacterStats>();
            stateManager = GetComponent<CharacterStateManager>();
            skillSlotsHandler = GetComponent<SkillSlotsHandler>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isCurrentPlayer)
            {                
                float delta = Time.deltaTime;                
                inventoryHandler.ActivateInventoryUI();
                InputHandler.instance.TickInput(delta);

                CameraHandler.instance.HandleCamera(delta);
                if (GameManager.instance.playerState != CharacterState.InMenu)
                {
                    if (selectedSkill == null)
                        selectedSkill = skillSlotsHandler.Move;
                    taticalMovement.UseSkill(selectedSkill, delta);
                }                
            }
            Debug.Log(characterStats.characterName+" Player Updated Completed");
        }
    }
}

