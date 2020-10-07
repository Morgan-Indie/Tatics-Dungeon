using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class IceSpikes : CastAlchemical
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            IceSpikes iceSpikes = _characterStats.gameObject.AddComponent<IceSpikes>();
            iceSpikes.characterStats = _characterStats;
            iceSpikes.animationHandler = _animationHandler;
            iceSpikes.taticalMovement = _taticalMovement;
            iceSpikes.skill = _skill;
            iceSpikes.combatUtils = _combatUtils;
            iceSpikes.skillAnimation = "SpellCastHand";

            iceSpikes.heatState = new HeatState(HeatValue.cold);            

            iceSpikes.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.waterDamage);
            iceSpikes.alchemicalDamgeType = CombatStatType.waterDamage;
            iceSpikes.normalDamage = new CombatStat(0, CombatStatType.normalDamage);
            iceSpikes.peirceDamage = new CombatStat(0, CombatStatType.pierceDamage);

            iceSpikes.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(iceSpikes.intScaleValue, StatModType.Flat);

            iceSpikes.alchemicalDamage.AddModifier(intScaling);
            return iceSpikes;
        }
    }
}