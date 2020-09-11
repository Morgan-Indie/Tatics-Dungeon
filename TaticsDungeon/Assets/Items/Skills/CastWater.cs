using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class CastWater : SkillAbstract
    {
        public float intScaleValue;
        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
                        TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        { return null; }

        public override void Cast(float delta, IntVector2 targetIndex) { Debug.Log("Need To Implement This Method In Child"); }

        public override void Excute(float delta, GridCell targetCell)
        {            
            AlchemyManager.Instance.ApplyLiquid(targetCell.alchemyState,LiquidPhaseState.Water);
            if (targetCell.occupyingObject != null)
            {
                CharacterStats targetStats = targetCell.occupyingObject.GetComponent<CharacterStats>();
                combatUtils.HandleAlchemicalSkill(targetStats, this);
                combatUtils.SetWaterInteractions(targetStats);
            }            
        }
    }
}