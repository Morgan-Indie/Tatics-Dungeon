using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class RangeAttack : SkillAbstract
    {
        public bool animationCompleted;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, Skill _skill)
        {
            RangeAttack rangeAttack = _characterStats.gameObject.AddComponent<RangeAttack>();
            rangeAttack.characterStats = _characterStats;
            rangeAttack.animationHandler = _animationHandler;
            rangeAttack.taticalMovement = _taticalMovement;
            rangeAttack.skill = _skill;
            return rangeAttack;
        }

        public override void Activate(float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            int distance = index.GetDistance(taticalMovement.currentIndex);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && distance <= 3 && distance!=0)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput)
                {
                    InputHandler.instance.tacticsXInput = false;
                    GridCell targetCell = taticalMovement.mapAdapter.GetCellByIndex(index);

                    Excute(delta, targetCell);
                }
            }
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            GameObject target = targetCell.GetOccupyingObject();
            if (target != null)
                characterStats.transform.LookAt(target.transform);
                characterStats.transform.Rotate(Quaternion.Euler(0f, 60f, 0f).eulerAngles);
                animationHandler.PlayTargetAnimation("Attack");
                characterStats.GetComponentInChildren<ArrowHolder>().target = target;
                characterStats.UseAP(skill.APcost);
        }
    }
}

