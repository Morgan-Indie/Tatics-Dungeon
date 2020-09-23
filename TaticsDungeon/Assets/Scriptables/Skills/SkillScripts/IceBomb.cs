using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class IceBomb : CastAlchemical
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats,
            AnimationHandler _animationHandler, TaticalMovement _taticalMovement, 
            CombatUtils _combatUtils, Skill _skill)
        {
            IceBomb iceBomb = _characterStats.gameObject.AddComponent<IceBomb>();
            iceBomb.characterStats = _characterStats;
            iceBomb.animationHandler = _animationHandler;
            iceBomb.taticalMovement = _taticalMovement;
            iceBomb.skill = _skill;
            iceBomb.combatUtils = _combatUtils;

            iceBomb.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.waterDamage);
            iceBomb.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(intScaleValue, StatModType.Flat);
            iceBomb.alchemicalDamage.AddModifier(intScaling);

            return iceBomb;
        }
    }
}