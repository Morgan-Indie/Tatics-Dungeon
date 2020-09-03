using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public static class MeleeAttack
    {
        public static void Activate(CharacterStats characterStats, AnimationHandler animationHandler,
            TaticalMovement taticalMovement, Skill skill, float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            int distance = Mathf.Abs(index.x - taticalMovement.currentIndex.x) + Mathf.Abs(index.y - taticalMovement.currentIndex.y);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && distance == 1)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput && 
                    characterStats.stateManager.characterState!= CharacterState.IsInteracting)
                {
                    InputHandler.instance.tacticsXInput = false;
                    GameObject target = taticalMovement.EnemyCheck(index);

                    if (target != null)
                    {
                        characterStats.transform.LookAt(target.transform);
                        animationHandler.PlayTargetAnimation("Attack");
                        characterStats.GetComponent<CombatUtils>().Attack(target);
                        characterStats.UseAP(skill.APcost);
                    }
                    else
                    {
                        Debug.LogError("No TargetDetected");
                    }
                }
            }
        }

        public static void Activate(CharacterStats characterStats, AnimationHandler animationHandler,
            TaticalMovement taticalMovement, Skill skill,PlayerManager target, float delta)
        {
            if (target != null && characterStats.stateManager.characterState != CharacterState.IsInteracting)
            {
                characterStats.transform.LookAt(target.transform);
                animationHandler.PlayTargetAnimation("Attack");
                characterStats.GetComponent<CombatUtils>().Attack(target.gameObject);
                characterStats.UseAP(skill.APcost);
            }
        }
    }       
}

