using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class CastFire : SkillAbstract
    {
        public float intScaleValue;

        public override void Cast(float delta, IntVector2 targetIndex)
        {
            List<GridCell> cells = CastableShapes.GetCastableCells(skill, targetIndex);

            characterStats.transform.LookAt(cells[0].transform);
            animationHandler.PlayTargetAnimation("SpellCastHand");
            characterStats.UseAP(skill.APcost);

            GridManager.Instance.RemoveAllHighlights();
            GameObject effect = Instantiate(skill.effectPrefab,
                taticalMovement.transform.position + Vector3.up * 1.5f + taticalMovement.transform.forward * 1f,
                Quaternion.identity);
            effect.GetComponent<VFXSpawns>().Initialize(cells, this);
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            bool cellBurn = false;
            if (targetCell.isFlammable || targetCell.alchemyState.liquidState == LiquidPhaseState.Oil)
            {
                AlchemyManager.Instance.ApplyHeat(targetCell.alchemyState);
                if (targetCell.alchemyState.fireState == FireState.Burning || targetCell.alchemyState.fireState == FireState.Inferno)
                {
                    targetCell.burnSource = this;
                    cellBurn = true;
                }
            }

            if (targetCell.occupyingObject != null)
            {
                CharacterStats targetStats = targetCell.occupyingObject.GetComponent<CharacterStats>();
                if (cellBurn)
                    combatUtils.SetFireInteractions(targetStats, targetCell, (int)alchemicalDamage.Value, true);
                else
                    combatUtils.SetFireInteractions(targetStats, this, (int)alchemicalDamage.Value);
                combatUtils.HandleAlchemicalSkill(targetStats, this);
            }           
        }
    }
}