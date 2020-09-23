using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class CastAlchemical : SkillAbstract
    {
        public float intScaleValue;
        List<GridCell> cells;

        public override void Cast(float delta, IntVector2 targetIndex)
        {
            if (skill.castType == CastType.Free)
            {
                cells = CastableShapes.GetCastableCells(skill, targetIndex);
            }
            else
            {
                cells = PinnedShapes.GetPinnedCells(skill, taticalMovement.currentIndex,targetIndex);
            }

            characterStats.transform.LookAt(cells[0].transform);
            animationHandler.PlayTargetAnimation(skillAnimation);
            characterStats.UseAP(skill.APcost);

            GridManager.Instance.RemoveAllHighlights();
            GameObject effect = Instantiate(skill.effectPrefab,
                taticalMovement.transform.position + Vector3.up * 1.5f + taticalMovement.transform.forward * 1f,
                Quaternion.identity);
            effect.GetComponent<VFXSpawns>().Initialize(cells, this, taticalMovement.currentIndex);
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            combatUtils.HandleAlchemicalSkillCell(targetCell, this);
            if (targetCell.occupyingObject != null)
            {
                combatUtils.HandleAlchemicalSkillCharacter(targetCell.occupyingObject.GetComponent<CharacterStats>(), this);
            }
        }
    }
}