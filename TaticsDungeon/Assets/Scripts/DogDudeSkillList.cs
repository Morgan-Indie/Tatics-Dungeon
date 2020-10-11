using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class DogDudeSkillList : MonoBehaviour
    {
        public List<Skill> skillObjects;
        public TaticalMovement taticalMovement;
        public CharacterStats characterStats;
        public AnimationHandler animationHandler;
        public CombatUtils combatUtils;

        public void Start()
        {
            taticalMovement = GetComponent<TaticalMovement>();
            characterStats = GetComponent<CharacterStats>();
            animationHandler = GetComponent<AnimationHandler>();
            combatUtils = GetComponent<CombatUtils>();
        }
    }
}

