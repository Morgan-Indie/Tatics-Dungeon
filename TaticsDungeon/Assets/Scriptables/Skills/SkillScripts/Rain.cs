using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class Rain : CastSubstance
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            Rain rain = _characterStats.gameObject.AddComponent<Rain>();
            rain.characterStats = _characterStats;
            rain.animationHandler = _animationHandler;
            rain.taticalMovement = _taticalMovement;
            rain.skill = _skill;
            rain.combatUtils = _combatUtils;
            rain.skillAnimation = "SpellCastHand";

            rain.heatState = new HeatState(HeatValue.neutral);
            rain._substance = new AlchemicalSubstance(AlchemicalState.liquid);            


            rain.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.waterDamage);
            rain.alchemicalDamgeType = CombatStatType.waterDamage;
            rain.normalDamage = new CombatStat(0, CombatStatType.normalDamage);
            rain.peirceDamage = new CombatStat(0, CombatStatType.pierceDamage);

            rain.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(rain.intScaleValue, StatModType.Flat);

            rain.alchemicalDamage.AddModifier(intScaling);
            return rain;
        }
    }
}