using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class MeleeAttack : CastPhysical
    {
        public GameObject target;
        public GridCell targetCell;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            MeleeAttack meleeAttack = _characterStats.gameObject.AddComponent<MeleeAttack>();
            meleeAttack.characterStats = _characterStats;
            meleeAttack.animationHandler = _animationHandler;
            meleeAttack.taticalMovement = _taticalMovement;
            meleeAttack.skill = _skill;
            meleeAttack.combatUtils = _combatUtils;

            meleeAttack.normalDamage = new CombatStat(0f, CombatStatType.normalDamage);
            meleeAttack.peirceDamage = new CombatStat(0f, CombatStatType.pierceDamage);
            meleeAttack.alchemicalDamage = new CombatStat(0f, CombatStatType.normalDamage);

            meleeAttack.normalDamage.AddModifier(new StatModifier(_characterStats.normalDamage.Value, StatModType.Flat));
            meleeAttack.peirceDamage.AddModifier(new StatModifier(_characterStats.pierceDamage.Value, StatModType.Flat));
            meleeAttack.alchemicalDamage.AddModifier(new StatModifier(_characterStats.pierceDamage.Value, StatModType.Flat));

            return meleeAttack;
        }

        public override void Cast(float delta, IntVector2 targetIndex)
        {
            List<GridCell> cells = CastableShapes.GetCastableCells(skill, targetIndex);
            targetCell = cells[0];

            target = targetCell.GetOccupyingObject();
            
            if (target != null)
            {
                characterStats.transform.LookAt(target.transform);
                characterStats.UseAP(skill.APcost);
                animationHandler.PlayTargetAnimation("Attack");
            }
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            combatUtils.DealDamage(target.GetComponent<CharacterStats>(),this);
            target.GetComponent<BloodVFX>().PlaySplashBloodEffects();
        }

        public override void Excute(float delta)
        {
            combatUtils.DealDamage(target.GetComponent<CharacterStats>(), this);
            target.GetComponent<BloodVFX>().PlaySplashBloodEffects();
        }
    }       
}

