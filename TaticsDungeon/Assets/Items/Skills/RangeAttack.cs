using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public static class RangeAttack
    {
        public static void Activate(CharacterStats characterStats, AnimationHandler animationHandler,
            TaticalMovement taticalMovement, Skill skill, float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            int distance = index.GetDistance(taticalMovement.currentIndex);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && distance <= 3)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput)
                {
                    InputHandler.instance.tacticsXInput = false;
                    GameObject target = taticalMovement.EnemyCheck(index);

                    if (target != null)
                    {
                        characterStats.transform.LookAt(target.transform);
                        characterStats.transform.Rotate(Quaternion.Euler(0f,60f,0f).eulerAngles);
                        animationHandler.PlayTargetAnimation("Attack");
                        characterStats.GetComponentInChildren<ArrowHolder>().target = target;
                        characterStats.UseAP(skill.APcost);                        
                    }
                }
            }
        }
    }
}

