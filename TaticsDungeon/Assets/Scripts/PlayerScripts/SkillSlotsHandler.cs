using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class SkillSlotsHandler : MonoBehaviour
    {
        [HideInInspector]
        private int allSlots = 10;
        private int enabledSlots;
        private SkillSlot[] slots;
        PlayerManager playerManager;
        CharacterStateManager stateManager;
        public GameObject skillPanel;
        public AnimationHandler animationHandler;
        public CombatUtils combatUtils;

        [Header("Required")]
        public GameObject skillPanelPrefab;
        public GameObject skillPanelBack;
        public Skill NormalAttack;
        public Skill Move;

        [Header("Test Only")]
        public TestAddNewSkills addNewSkills;

        public void Start()
        {
            skillPanel = Instantiate(skillPanelPrefab) as GameObject;
            skillPanel.transform.SetParent(skillPanelBack.transform,false);
            slots = skillPanel.GetComponentsInChildren<SkillSlot>();
            addNewSkills = GetComponent<TestAddNewSkills>();
            combatUtils = GetComponent<CombatUtils>();

            foreach (SkillSlot slot in slots)
            {
                if (slot.skill == null)
                    slot.empty = true;
            }

            stateManager = GetComponent<CharacterStateManager>();
            playerManager = GetComponent<PlayerManager>();
            animationHandler = GetComponent<AnimationHandler>();

            AddSkill(Move);
            AddSkill(NormalAttack);
            addNewSkills.addSkill(this);

            if (playerManager.isCurrentPlayer)
                skillPanel.SetActive(true);
            else
                skillPanel.SetActive(false);
        }        

        public void AddSkill(Skill skill)
        {
            for (int i = 0; i < allSlots; i++)
            {
                if (slots[i].empty)
                {
                    slots[i].skill = skill;
                    slots[i].UpdateSlot(playerManager.characterStats,animationHandler,
                        playerManager.taticalMovement, combatUtils);
                    slots[i].empty = false;
                    return;
                }
            }
        }
    }
}

