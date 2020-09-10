using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCross : SkillAbstract
    {
        public float _firedamage = 20f;
        public float intScaleValue;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, Skill _skill)
        {
            FlameCross flameCross = _characterStats.gameObject.AddComponent<FlameCross>();
            flameCross.characterStats = _characterStats;
            flameCross.animationHandler = _animationHandler;
            flameCross.taticalMovement = _taticalMovement;
            flameCross.skill = _skill;

            flameCross.alchemicalDamage = new CombatStat(_firedamage, CombatStatType.fireDamage);

           // flameCross.intScaleValue = skill.attributeScaleModDict[AttributeType.intelligence].Value * _characterStats.Intelligence.Value;
            //StatModifier intScaling = new StatModifier(intScaleValue, StatModType.Flat);

          //  flameCross.alchemicalDamage.AddModifier(intScaling);
           // Debug.Log(flameCross.alchemicalDamage.Value);
            return flameCross;
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
                    List<GridCell> cells = CastableShapes.GetCastableCells(skill, index);
                    GameObject effect = Instantiate(skill.effectPrefab, taticalMovement.transform.position + Vector3.up * 1.5f + taticalMovement.transform.forward * 1f, Quaternion.identity);
                    effect.GetComponent<FlameCrossSpawn>().Initalize(cells, this);
                    characterStats.transform.LookAt(cells[0].transform);
                    GridManager.Instance.RemoveAllHighlights();
                }
            }
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            animationHandler.PlayTargetAnimation("Attack");

            characterStats.UseAP(skill.APcost);

            AlchemyManager.Instance.ApplyHeat(targetCell.alchemyState);
            if (targetCell.occupyingObject != null)
                characterStats.GetComponent<CombatUtils>().OffensiveSpell(targetCell.occupyingObject, this);
        }
    }
}