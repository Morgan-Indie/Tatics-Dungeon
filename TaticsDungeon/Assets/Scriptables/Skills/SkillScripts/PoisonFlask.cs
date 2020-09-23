using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class PoisonFlask : CastSubstance
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            PoisonFlask poisonFlask = _characterStats.gameObject.AddComponent<PoisonFlask>();
            poisonFlask.characterStats = _characterStats;
            poisonFlask.animationHandler = _animationHandler;
            poisonFlask.taticalMovement = _taticalMovement;
            poisonFlask.skill = _skill;
            poisonFlask.combatUtils = _combatUtils;
            poisonFlask.skillAnimation = "Toss";

            poisonFlask.heatValue = HeatValue.neutral;
            poisonFlask.substance = new AlchemicalSubstance(AlchemicalState.liquid);
            poisonFlask.substance.AddAuxState(StatusEffect.Poisoned);

            poisonFlask.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.poisonDamage);
            poisonFlask.alchemicalDamgeType = CombatStatType.poisonDamage;
            poisonFlask.normalDamage = new CombatStat(0, CombatStatType.normalDamage);
            poisonFlask.peirceDamage = new CombatStat(0, CombatStatType.pierceDamage);

            poisonFlask.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(poisonFlask.intScaleValue, StatModType.Flat);

            poisonFlask.alchemicalDamage.AddModifier(intScaling);
            return poisonFlask;
        }
    }
}