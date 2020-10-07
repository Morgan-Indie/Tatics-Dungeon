using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class HealingHands : CastPhysical
    {
        public GameObject target;
        public GridCell targetCell;
        public Stat HealValue;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            HealingHands healingHands = _characterStats.gameObject.AddComponent<HealingHands>();
            healingHands.characterStats = _characterStats;
            healingHands.animationHandler = _animationHandler;
            healingHands.taticalMovement = _taticalMovement;
            healingHands.skill = _skill;
            healingHands.combatUtils = _combatUtils;
            healingHands.HealValue = new Stat(_skill.damage);
            healingHands.scaleValue = _characterStats.Vitality.Value * _skill._scaleValue;

            StatModifier scaleMod = new StatModifier(healingHands.scaleValue, StatModType.Flat);
            healingHands.HealValue.AddModifier(scaleMod);

            return healingHands;
        }

        public override void Cast(float delta, IntVector2 targetIndex)
        {
            List<GridCell> cells = CastableShapes.GetCastableCells(skill, targetIndex);
            targetCell = cells[0];

            target = targetCell.occupyingObject;

            if (target != null)
            {
                characterStats.transform.LookAt(target.transform);
                characterStats.UseAP(skill.APcost);
                animationHandler.PlayTargetAnimation("SpellCastHand");
                Excute(delta);
            }
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            target.GetComponent<CharacterStats>().Heal((int)HealValue.Value);
            ActivateVFX.Instance.ActivateHealingEffect(target);            
        }

        public override void Excute(float delta)
        {
            target.GetComponent<CharacterStats>().Heal((int)HealValue.Value);
            ActivateVFX.Instance.ActivateHealingEffect(target);
        }
    }
}

