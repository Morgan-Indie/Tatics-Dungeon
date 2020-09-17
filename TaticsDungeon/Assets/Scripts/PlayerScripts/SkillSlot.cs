using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class SkillSlot : MonoBehaviour
    {
        [Header("Not Required")]
        public bool empty;
        public Skill skill;
        public Transform slotIconPanel;
        public SkillAbstract skillScript;        

        [Header("Required")]
        public Sprite defaultSprite;

        private void Awake()
        {
            slotIconPanel = transform.GetChild(0);            
        }

        public void UpdateSlot(CharacterStats characterStats, AnimationHandler animationHandler,
            TaticalMovement taticalMovement,CombatUtils combatUtils)
        {
            if (skill == null)
                slotIconPanel.GetComponent<Image>().sprite = defaultSprite;
            else
            {
                slotIconPanel.GetComponent<Image>().sprite = skill.icon;
                skillScript = skill.skillScriptObject.GetComponent<SkillAbstract>();
                skillScript = skillScript.AttachSkill(characterStats, animationHandler, 
                    taticalMovement, combatUtils,skill);
            }
        }

        public void OnClick()
        {
            if (skillScript.skill.type == SkillType.Move)
                GameManager.instance.currentCharacter.taticalMovement.SetCurrentNavDict();
            GameManager.instance.SwitchSkill(skillScript);
        }
    }
}

