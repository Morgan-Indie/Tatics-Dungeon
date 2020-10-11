using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class AISkillSlotHandler : MonoBehaviour
    {
        [HideInInspector]
        private int allSlots = 10;
        private SkillSlot[] slots;
        CharacterStateManager stateManager;
        public GameObject skillPanel;
        public AnimationHandler animationHandler;
        public CombatUtils combatUtils;
        public CharacterStats characterStats;
        public DogDudeSkillList skillList;
        public List<SkillAbstract> skills;

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
            skillPanel.transform.SetParent(skillPanelBack.transform, false);
            slots = skillPanel.GetComponentsInChildren<SkillSlot>();
            addNewSkills = GetComponent<TestAddNewSkills>();
            combatUtils = GetComponent<CombatUtils>();
            skillList = GetComponent<DogDudeSkillList>();

            foreach (SkillSlot slot in slots)
            {
                if (slot.skill == null)
                    slot.empty = true;
            }

            stateManager = GetComponent<CharacterStateManager>();
            animationHandler = GetComponent<AnimationHandler>();
            characterStats = GetComponent<CharacterStats>();
            AddSkills();
        }

        public void AddSkills()
        {
            foreach (Skill skill in skillList.skillObjects)
            {
                SkillAbstract skillScript = skill.skillScriptObject.GetComponent<SkillAbstract>();
                skills.Add(skillScript);
                AddSkill(skill);
            }
        }

        public void AddSkill(Skill skill)
        {
            for (int i = 0; i < allSlots; i++)
            {
                if (slots[i].empty)
                {
                    slots[i].skill = skill;
                    slots[i].UpdateSlot(characterStats, animationHandler,
                        characterStats.taticalMovement, combatUtils);
                    slots[i].empty = false;
                    return;
                }
            }
        }

        public void UpdateCoolDowns()
        {
            foreach (SkillSlot slot in slots)
            {
                if (!slot.empty)
                {
                    if (slot.skillCoolDownTurns == 0)
                    {
                        slot.EnableSkill();
                    }
                    else
                    {
                        slot.DisableSkill();
                        slot.skillCoolDownTurns -= 1;
                    }
                }
            }
        }
    }
}

