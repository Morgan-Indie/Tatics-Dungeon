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
        public Transform slotIconMask;
        public SkillAbstract skillScript; 
        public int skillCoolDownTurns;

        [Header("Required")]
        public Sprite defaultSprite;

        private void Awake()
        {
            slotIconPanel = transform.GetChild(0);
            slotIconMask = transform.GetChild(1);

        }

        public void DisableSkill()
        {
            Color temp = slotIconMask.GetComponent<Image>().color;
            temp.a = .2f;
            slotIconMask.GetComponent<Image>().color=temp;
            GetComponent<Button>().enabled = false;
        }

        public void EnableSkill()
        {
            Color temp = slotIconMask.GetComponent<Image>().color;
            temp.a = 0f;
            slotIconMask.GetComponent<Image>().color = temp;
            GetComponent<Button>().enabled = true;
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
                    taticalMovement, combatUtils,skill,this);
                skillCoolDownTurns = 0;
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

