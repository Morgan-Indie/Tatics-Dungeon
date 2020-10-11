using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class LightingStrike : CastAlchemical
    {
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill, SkillSlot _slot)
        {
            LightingStrike lightingStrike = _characterStats.gameObject.AddComponent<LightingStrike>();
            lightingStrike.characterStats = _characterStats;
            lightingStrike.animationHandler = _animationHandler;
            lightingStrike.taticalMovement = _taticalMovement;
            lightingStrike.skill = _skill;
            lightingStrike.combatUtils = _combatUtils;
            lightingStrike.skillAnimation = "SpellCastHand";
            lightingStrike.heatState = new HeatState(HeatValue.neutral);
            lightingStrike.slot = _slot;

            lightingStrike.alchemicalDamage = new CombatStat(_skill.damage, CombatStatType.shockDamage);
            lightingStrike.alchemicalDamgeType = CombatStatType.shockDamage;
            lightingStrike.normalDamage = new CombatStat(0, CombatStatType.normalDamage);
            lightingStrike.peirceDamage = new CombatStat(0, CombatStatType.pierceDamage);

            lightingStrike.intScaleValue = skill._scaleValue * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(lightingStrike.intScaleValue, StatModType.Flat);

            lightingStrike.alchemicalDamage.AddModifier(intScaling);
            return lightingStrike;
        }
    }
}