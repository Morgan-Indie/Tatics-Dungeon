using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public enum PlayerNumber
    {
        player1,player2,player3,player4
    }

    public class PlayerManager : CharacterManager
    {
        [Header("Required")]
        public PlayerNumber playerNumber;
        public CharacterClass characterClass;
        public GameObject statusPanel;
        public string characterName;

        [Header("Auto Filled GameObjects")]
        public PlayerMovement playerMovement;
        public InventoryHandler inventoryHandler;

        private void Awake()
        {
            inventoryHandler = GetComponent<InventoryHandler>();
            stateManager = GetComponent<CharacterStateManager>();
            animationHandler = GetComponent<AnimationHandler>();
            playerMovement = GetComponent<PlayerMovement>();
            AP = GetComponent<CharacterAP>();
            health = GetComponent<CharacterHealth>();
            location = GetComponent<CharacterLocation>();
            stats = GetComponent<CharacterStats>();
            combatStats = GetComponent<CharacterCombatStats>();
        }

        private void Start()
        {
            panelName = statusPanel.GetComponentInChildren<Text>();
            panelName.text = characterName;
        }

        public override void DisableCharacter()
        {
            GameManager.instance.playersDict.Remove(characterName);
            gameObject.SetActive(false);
<<<<<<< Updated upstream
=======
        }

        public void UseSkill(SkillAbstract skillScript, float delta)
        {
            skillScript.Activate(delta);
>>>>>>> Stashed changes
        }

        public override void HandleDeath()
        {
            stateManager.characterState = CharacterState.Dead;
            animationHandler.PlayTargetAnimation("Death");
        }

        // Update is called once per frame
        public override void CharacterUpdate(float delta)
        {
<<<<<<< Updated upstream
            if (isCurrentPlayer)
            {                
=======
            if (isCurrentCharacter && stateManager.characterState != CharacterState.Disabled)
            {                                
>>>>>>> Stashed changes
                inventoryHandler.ActivateInventoryUI();
                if (GameManager.instance.gameState != GameState.InMenu)
                {
                    if (selectedSkill == null || selectedSkill.skill.type == SkillType.Move)
<<<<<<< Updated upstream
                        taticalMovement.ExcuteMovement(delta);
                    else
                        taticalMovement.UseSkill(selectedSkill, delta);
                }                
=======
                        playerMovement.ExcuteMovement(delta);
                    else
                        UseSkill(selectedSkill, delta);
                }
>>>>>>> Stashed changes
            }
        }
    }
}

