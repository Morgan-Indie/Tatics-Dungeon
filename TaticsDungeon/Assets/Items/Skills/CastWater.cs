using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CastWater : SkillAbstract
    {
        public List<GameObject> targets;
        public float intScaleValue;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, Skill _skill)
        {
            CastWater castWater = _characterStats.gameObject.AddComponent<CastWater>();
            castWater.characterStats = _characterStats;
            castWater.animationHandler = _animationHandler;
            castWater.taticalMovement = _taticalMovement;
            castWater.skill = _skill;

            castWater.alchemicalDamage = new CombatStat(_skill._scaleValueWaterDamage, CombatStatType.waterDamage);

            castWater.intScaleValue = _skill.attributeScaleModDict[AttributeType.intelligence].Value * _characterStats.Intelligence.Value;
            StatModifier intScaling = new StatModifier(castWater.intScaleValue, StatModType.Flat);

            castWater.alchemicalDamage.AddModifier(intScaling);
            return castWater;
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
                AlchemyManager.Instance.ApplyLiquid(cell.alchemyState,LiquidPhaseState.Water);
                if (cell.occupyingObject != null)
                    characterStats.GetComponent<CombatUtils>().OffensiveSpell(cell.occupyingObject, this);
            }
        }
    }
}