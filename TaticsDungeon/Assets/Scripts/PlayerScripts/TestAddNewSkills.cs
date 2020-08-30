using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{    
    public class TestAddNewSkills : MonoBehaviour
    {
        public Skill[] skills;

        public void addSkill(SkillSlotsHandler skillSlotsHandler)
        {
            foreach(Skill skill in skills)
                skillSlotsHandler.AddSkill(skill);
        }
    }
}
