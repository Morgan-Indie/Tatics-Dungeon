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
                cells = CastableShapes.GetCastableCells(skill, targetIndex);                
            else
                cells = PinnedShapes.GetPinnedCells(skill, character.location.currentIndex,targetIndex);

            character.transform.LookAt(cells[0].transform);
            character.animationHandler.PlayTargetAnimation(skillAnimation);
            character.AP.UseAP(skill.APcost);

            slot.skillCoolDownTurns += skill.coolDown;
            slot.DisableSkill();
            GridManager.Instance.RemoveAllHighlights();

            GameObject effect = Instantiate(skill.effectPrefab,
                character.transform.position + Vector3.up * 1.5f + character.transform.forward * 1f,
                Quaternion.identity);
            effect.GetComponent<VFXSpawns>().Initialize(cells, this, character.location.currentIndex);
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            if (targetCell.occupyingObject != null)
                CombatUtils.Instance.HandleAlchemicalSkillCharacter(targetCell.occupyingObject.GetComponent<CharacterManager>(),
                    targetCell,this);
            else
                CombatUtils.Instance.HandleAlchemicalSkillCell(targetCell, this);
        }
    }
}