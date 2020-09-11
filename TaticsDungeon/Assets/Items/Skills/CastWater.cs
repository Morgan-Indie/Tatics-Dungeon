using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class CastWater : SkillAbstract
    {
        public float intScaleValue;

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