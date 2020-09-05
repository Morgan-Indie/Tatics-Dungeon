using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class RangeAttack : SkillAbstract
    {
        public override void Activate(CharacterStats characterStats,
                    AnimationHandler animationHandler, TaticalMovement taticalMovement,
                    float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            int distance = index.GetDistance(taticalMovement.currentIndex);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && distance <= 3)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput)
                {
                    InputHandler.instance.tacticsXInput = false;
                    GridCell targetCell = taticalMovement.mapAdapter.GetCellByIndex(index);

                    Excute(characterStats,animationHandler,taticalMovement,delta, targetCell);
                }
            }
        }

        public override void Excute(CharacterStats characterStats,
            AnimationHandler animationHandler, TaticalMovement taticalMovement,
            float delta, GridCell targetCell)
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

