using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class EnergyBall : CastAlchemical
    {
        public GameObject target;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler, 
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill, SkillSlot _slot)
        {
            EnergyBall energyBall = _characterStats.gameObject.AddComponent<EnergyBall>();
            energyBall.characterStats = _characterStats;
            energyBall.animationHandler = _animationHandler;
            energyBall.taticalMovement = _taticalMovement;
            energyBall.skill = _skill;
            energyBall.combatUtils = _combatUtils;

            energyBall.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.shockDamage);

            energyBall.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(intScaleValue, StatModType.Flat);

            energyBall.alchemicalDamage.AddModifier(intScaling);
            return energyBall;
        }
    }
}