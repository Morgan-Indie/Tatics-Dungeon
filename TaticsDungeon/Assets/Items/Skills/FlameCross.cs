using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCross : CastFire
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            FlameCross flameCross = _characterStats.gameObject.AddComponent<FlameCross>();
            flameCross.characterStats = _characterStats;
            flameCross.animationHandler = _animationHandler;
            flameCross.taticalMovement = _taticalMovement;
            flameCross.skill = _skill;
            flameCross.combatUtils = _combatUtils;

            flameCross.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.fireDamage);

            flameCross.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(intScaleValue, StatModType.Flat);

            flameCross.alchemicalDamage.AddModifier(intScaling);
            // Debug.Log(flameCross.alchemicalDamage.Value);
            return flameCross;
        }
    }
}