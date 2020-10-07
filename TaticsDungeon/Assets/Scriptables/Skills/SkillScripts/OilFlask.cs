using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class OilFlask : CastSubstance
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            OilFlask oilFlask = _characterStats.gameObject.AddComponent<OilFlask>();
            oilFlask.characterStats = _characterStats;
            oilFlask.animationHandler = _animationHandler;
            oilFlask.taticalMovement = _taticalMovement;
            oilFlask.skill = _skill;
            oilFlask.combatUtils = _combatUtils;
            oilFlask.skillAnimation = "Toss";

            oilFlask.heatState = new HeatState(HeatValue.neutral);
            oilFlask.substance = new AlchemicalSubstance(AlchemicalState.liquid);
            oilFlask.substance.AddAuxState(StatusEffect.Oiled);

            oilFlask.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.normalDamage);
            oilFlask.alchemicalDamgeType = CombatStatType.waterDamage;
            oilFlask.normalDamage = new CombatStat(0, CombatStatType.normalDamage);
            oilFlask.peirceDamage = new CombatStat(0, CombatStatType.pierceDamage);

            oilFlask.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(oilFlask.intScaleValue, StatModType.Flat);

            oilFlask.alchemicalDamage.AddModifier(intScaling);
            return oilFlask;
        }
    }
}