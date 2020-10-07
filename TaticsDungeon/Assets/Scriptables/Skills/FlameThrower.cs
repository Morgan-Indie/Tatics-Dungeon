using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameThrower : CastAlchemical
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            FlameThrower flameThrower = _characterStats.gameObject.AddComponent<FlameThrower>();
            flameThrower.characterStats = _characterStats;
            flameThrower.animationHandler = _animationHandler;
            flameThrower.taticalMovement = _taticalMovement;
            flameThrower.skill = _skill;
            flameThrower.combatUtils = _combatUtils;
            flameThrower.skillAnimation = "SpellCastHand";

            flameThrower.heatState = new HeatState(HeatValue.hot);

            flameThrower.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.fireDamage);
            flameThrower.alchemicalDamgeType = CombatStatType.fireDamage;
            flameThrower.normalDamage = new CombatStat(0, CombatStatType.normalDamage);
            flameThrower.peirceDamage = new CombatStat(0, CombatStatType.pierceDamage);

            flameThrower.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(flameThrower.intScaleValue, StatModType.Flat);

            flameThrower.alchemicalDamage.AddModifier(intScaling);
            return flameThrower;
        }
    }
}