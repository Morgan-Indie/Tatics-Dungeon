using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CastFire : SkillAbstract
    {
        public List<GameObject> targets;
        public float _firedamage = 20f;
        public float intScaleValue;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, Skill _skill)
        {
            CastFire castFire = _characterStats.gameObject.AddComponent<CastFire>();
            castFire.characterStats = _characterStats;
            castFire.animationHandler = _animationHandler;
            castFire.taticalMovement = _taticalMovement;
            castFire.skill = _skill;

            castFire.alchemicalDamage = new CombatStat(_firedamage, CombatStatType.fireDamage);

            castFire.intScaleValue = skill.attributeScaleModDict[AttributeType.intelligence].Value * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(intScaleValue, StatModType.Flat);

            castFire.alchemicalDamage.AddModifier(intScaling);
            return castFire;
        }

        public override void Activate(float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            GridManager.Instance.HighlightCastableRange(taticalMovement.currentIndex, index, skill);
            int distance = taticalMovement.currentIndex.GetDistance(index);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput &&
                    characterStats.stateManager.characterState != CharacterState.IsInteracting)
                {
                    InputHandler.instance.tacticsXInput = false;
                    GridCell targetCell = taticalMovement.mapAdapter.GetCellByIndex(index);

                    Excute(delta, targetCell);
                }
            }
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            characterStats.transform.LookAt(targetCell.transform);
            animationHandler.PlayTargetAnimation("SpellCastHand");
            
            characterStats.UseAP(skill.APcost);

            GridManager.Instance.RemoveAllHighlights();
            List<GridCell> cells = CastableShapes.GetCastableCells(skill, targetCell.index);
            foreach (GridCell cell in cells)
            {
                AlchemyManager.Instance.ApplyHeat(cell.alchemyState);
                if (cell.occupyingObject != null)
                    characterStats.GetComponent<CombatUtils>().OffensiveSpell(cell.occupyingObject, this);
            }
        }
    }
}