using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class MeleeAttack : CastPhysical
    {
        public GameObject target;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            MeleeAttack meleeAttack = _characterStats.gameObject.AddComponent<MeleeAttack>();
            meleeAttack.characterStats = _characterStats;
            meleeAttack.animationHandler = _animationHandler;
            meleeAttack.taticalMovement = _taticalMovement;
            meleeAttack.skill = _skill;
            meleeAttack.combatUtils = _combatUtils;
            return meleeAttack;
        }

        public override void Cast(float delta, IntVector2 targetIndex)
        {
            List<GridCell> cells = CastableShapes.GetCastableCells(skill, targetIndex);
            GridCell targetCell = cells[0];

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
            combatUtils.PhyiscalAttack(target);
        }

        public override void Excute(float delta)
        {
            combatUtils.PhyiscalAttack(target);
        }
    }       
}

