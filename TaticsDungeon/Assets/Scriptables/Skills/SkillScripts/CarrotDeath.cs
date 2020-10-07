using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CarrotDeath : CastAlchemical
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            CarrotDeath carrotDeath = _characterStats.gameObject.AddComponent<CarrotDeath>();
            carrotDeath.characterStats = _characterStats;
            carrotDeath.animationHandler = _animationHandler;
            carrotDeath.taticalMovement = _taticalMovement;
            carrotDeath.skill = _skill;
            carrotDeath.combatUtils = _combatUtils;
            carrotDeath.skillAnimation = "SpellCastHand";

            carrotDeath.heatState = new HeatState(HeatValue.hot);

            carrotDeath.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.fireDamage);
            carrotDeath.alchemicalDamgeType = CombatStatType.fireDamage;
            carrotDeath.normalDamage = new CombatStat(0, CombatStatType.normalDamage);
            carrotDeath.peirceDamage = new CombatStat(0, CombatStatType.pierceDamage);

            carrotDeath.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(carrotDeath.intScaleValue, StatModType.Flat);

            carrotDeath.alchemicalDamage.AddModifier(intScaling);
            return carrotDeath;
        }
    }
}