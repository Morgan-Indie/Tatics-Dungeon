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

<<<<<<< Updated upstream
        public void UpdateSlot(CharacterStats characterStats, AnimationHandler animationHandler,
            TaticalMovement taticalMovement)
=======
        public void UpdateSlot(CharacterStats _characterStats,
            AnimationHandler _animationHandler, TaticalMovement _taticalMovement)
>>>>>>> Stashed changes
        {
            if (skill == null)
                slotIconPanel.GetComponent<Image>().sprite = defaultSprite;
            else
            {
                slotIconPanel.GetComponent<Image>().sprite = skill.icon;
                skillScript = skill.skillScriptObject.GetComponent<SkillAbstract>();
<<<<<<< Updated upstream
                skillScript = skillScript.AttachSkill(characterStats, animationHandler, taticalMovement,skill);
=======
                skillScript.AttachToCharacter(_characterStats, _animationHandler, _taticalMovement);
>>>>>>> Stashed changes
            }
        }

        public void OnClick()
        {
            GameManager.instance.SwitchSkill(skillScript);
        }
    }
}

