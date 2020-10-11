using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCross : CastAlchemical
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill,SkillSlot _slot)
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

        public override void Cast(float delta, IntVector2 targetIndex)
        {
            base.Cast(delta, targetIndex);
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            base.Excute(delta, targetCell);
        }
    }
}