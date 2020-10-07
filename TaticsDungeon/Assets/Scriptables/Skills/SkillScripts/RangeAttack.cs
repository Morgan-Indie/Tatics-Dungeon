using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class RangeAttack : CastPhysical
    {
        public GameObject target;
        public ArrowHolder arrowHolder;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils,Skill _skill)
        {
            RangeAttack rangeAttack = _characterStats.gameObject.AddComponent<RangeAttack>();
            rangeAttack.characterStats = _characterStats;
            rangeAttack.animationHandler = _animationHandler;
            rangeAttack.taticalMovement = _taticalMovement;
            rangeAttack.skill = _skill;
            rangeAttack.combatUtils = _combatUtils;
            rangeAttack.arrowHolder = _taticalMovement.GetComponent<ArrowHolder>();
            rangeAttack.arrowHolder.rangeAttack = rangeAttack;

            rangeAttack.normalDamage = new CombatStat(0f, CombatStatType.normalDamage);
            rangeAttack.peirceDamage = new CombatStat(0f, CombatStatType.pierceDamage);
            rangeAttack.alchemicalDamage = new CombatStat(0f, CombatStatType.normalDamage);

            rangeAttack.normalDamage.AddModifier(new StatModifier(_characterStats.normalDamage.Value, StatModType.Flat));
            rangeAttack.peirceDamage.AddModifier(new StatModifier(_characterStats.pierceDamage.Value, StatModType.Flat));
            rangeAttack.alchemicalDamage.AddModifier(new StatModifier(_characterStats.pierceDamage.Value, StatModType.Flat));

            return rangeAttack;
        }

        public override void Cast(float delta, IntVector2 targetIndex)
        {
            List<GridCell> cells = CastableShapes.GetCastableCells(skill, targetIndex);
            GridCell targetCell = cells[0];

            target = targetCell.occupyingObject;

            if (target != null)
            {
                characterStats.transform.LookAt(target.transform);
                characterStats.transform.Rotate(Quaternion.Euler(0f, 60f, 0f).eulerAngles);
                animationHandler.PlayTargetAnimation("Attack");
                arrowHolder.targetCell = targetCell;
                arrowHolder.target = target;
                characterStats.UseAP(skill.APcost);
            }
        }

        public override void Excute(float delta, GridCell targetCell)
        {

        }

        public override void Excute(float delta)
        {
            combatUtils.DealDamage(target.GetComponent<CharacterStats>(),this);
            target.GetComponent<BloodVFX>().PlayPeirceBloodEffects();
        }
    }
}

