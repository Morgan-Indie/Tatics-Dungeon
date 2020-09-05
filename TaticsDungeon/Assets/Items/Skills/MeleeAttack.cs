using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class MeleeAttack : SkillAbstract
    {
        public bool animationCompleted;

        public override void Activate(CharacterStats characterStats,
            AnimationHandler animationHandler, TaticalMovement taticalMovement,
            float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            int distance = Mathf.Abs(index.x - taticalMovement.currentIndex.x) + Mathf.Abs(index.y - taticalMovement.currentIndex.y);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && distance == 1)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput && 
                    characterStats.stateManager.characterState!= CharacterState.IsInteracting)
                {
                    InputHandler.instance.tacticsXInput = false;
                    GridCell targetCell = taticalMovement.mapAdapter.GetCellByIndex(index);

                    Excute(characterStats,animationHandler, taticalMovement, delta, targetCell);
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
                animationHandler.PlayTargetAnimation("Attack");
                characterStats.GetComponent<CombatUtils>().Attack(target);
                characterStats.UseAP(skill.APcost);
        }
    }       
}

