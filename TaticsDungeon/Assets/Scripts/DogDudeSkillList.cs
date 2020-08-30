using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class DogDudeSkillList : MonoBehaviour
    {
        public Skill meleeAttack;
        public Dictionary<SkillType, Skill> skillDict = new Dictionary<SkillType, Skill>();

        public void Start()
        {
            skillDict.Add(meleeAttack.type,meleeAttack);
        }
    }
}

