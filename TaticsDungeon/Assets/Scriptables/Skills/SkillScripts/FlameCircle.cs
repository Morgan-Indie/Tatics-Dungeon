using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCircle : CastAlchemical
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats,
            AnimationHandler _animationHandler, TaticalMovement _taticalMovement,CombatUtils _combatUtils, Skill _skill)
        {
            FlameCircle flameCircle = _characterStats.gameObject.AddComponent<FlameCircle>();
            flameCircle.characterStats = _characterStats;
            flameCircle.animationHandler = _animationHandler;
            flameCircle.taticalMovement = _taticalMovement;
            flameCircle.skill = _skill;
            flameCircle.combatUtils = _combatUtils;

            flameCircle.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.fireDamage);
            flameCircle.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(flameCircle.intScaleValue, StatModType.Flat);
            flameCircle.alchemicalDamage.AddModifier(intScaling);

            return flameCircle;
        }      
    }
}