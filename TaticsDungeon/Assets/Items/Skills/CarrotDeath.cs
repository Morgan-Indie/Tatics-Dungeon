using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CarrotDeath : CastFire
    {
        public bool animationCompleted;
        public GameObject target;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats,
            AnimationHandler _animationHandler, TaticalMovement _taticalMovement, CombatUtils _combatUtils,Skill _skill)
        {
            CarrotDeath carrotDeath = _characterStats.gameObject.AddComponent<CarrotDeath>();
            carrotDeath.characterStats = _characterStats;
            carrotDeath.animationHandler = _animationHandler;
            carrotDeath.taticalMovement = _taticalMovement;
            carrotDeath.skill = _skill;
            carrotDeath.combatUtils = _combatUtils;
            return carrotDeath;
        }
    }
}