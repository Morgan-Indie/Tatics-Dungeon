using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public abstract class CastSubstance : SkillAbstract
    {
        public AlchemicalSubstance _substance;

        public AlchemicalSubstance substance { get { return new AlchemicalSubstance(_substance);}}

        public override void Cast(float delta, IntVector2 targetIndex)
        {
            List<GridCell> cells = CastableShapes.GetCastableCells(skill, targetIndex);

            character.transform.LookAt(cells[0].transform);
            character.animationHandler.PlayTargetAnimation(skillAnimation);
            character.AP.UseAP(skill.APcost);

            if (slot != null)
            {
                slot.skillCoolDownTurns += skill.coolDown;
                slot.DisableSkill();
                GridManager.Instance.RemoveAllHighlights();
                character.selectedSkill = null;
            }

            GameObject effect = Instantiate(skill.effectPrefab,
                character.transform.position + Vector3.up * 1.5f + character.transform.forward * 1f,
                Quaternion.identity);
            effect.GetComponent<VFXSpawns>().Initialize(cells, this, character.location.currentIndex);
        }

        public override void Excute(float delta, GridCell targetCell)
        {           
            if (targetCell.occupyingObject != null)
                CombatUtils.Instance.HandleAlchemicalSkillCharacter(targetCell.occupyingObject.GetComponent<CharacterManager>(), targetCell,this);
            else
                CombatUtils.Instance.HandleAlchemicalSkillCell(targetCell, this);
        }
    }
}
