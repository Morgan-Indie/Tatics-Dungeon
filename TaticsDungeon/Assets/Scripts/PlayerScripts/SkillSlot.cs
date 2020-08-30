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

        [Header("Required")]
        public Sprite defaultSprite;

        private void Awake()
        {
            slotIconPanel = transform.GetChild(0);
        }

        public void UpdateSlot()
        {
            if (skill == null)
                slotIconPanel.GetComponent<Image>().sprite = defaultSprite;
            else
                slotIconPanel.GetComponent<Image>().sprite = skill.icon;
        }

        public void OnClick()
        {
            GameManager.instance.SwitchSkill(skill);
        }
    }
}

