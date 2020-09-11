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
        public SkillAbstract selectedSkill=null;

        private void Start()
        {
            playerTransform = GetComponent<Transform>();
            inventoryHandler = GetComponent<InventoryHandler>();
            taticalMovement = GetComponent<TaticalMovement>();
            characterStats = GetComponent<CharacterStats>();
            stateManager = GetComponent<CharacterStateManager>();
            skillSlotsHandler = GetComponent<SkillSlotsHandler>();
        }

        public void DisableCharacter()
        {
            GameManager.instance.playersDict.Remove(characterStats.characterName);
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        public void PlayerUpdate(float delta)
        {
            if (isCurrentPlayer && !stateManager.statusEffects.Contains(StatusEffect.Frozen))
            {                
                inventoryHandler.ActivateInventoryUI();
                Debug.Log(selectedSkill);

                if (GameManager.instance.gameState != GameState.InMenu)
                {
                    if (selectedSkill == null || selectedSkill.skill.type == SkillType.Move)
                        taticalMovement.ExcuteMovement(delta);
                    else
                    {
                        Debug.Log(selectedSkill);
                        taticalMovement.UseSkill(selectedSkill, delta);
                    }
                }                
            }
        }
    }
}

