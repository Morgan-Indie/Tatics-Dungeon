using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public static class ShieldCharge
    {
        public static GameObject target=null;
        static Vector3 targetDirection;
        static Vector3 targetPos;

        public static void Activate(CharacterStats characterStats, AnimationHandler animationHandler,
            TaticalMovement taticalMovement, Skill skill, float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            int distance = index.GetDistance(taticalMovement.currentIndex);
            CharacterStateManager stateManager = characterStats.GetComponent<CharacterStateManager>();
            float percentNormalDamage = skill.combatStatScaleDict[CombatStatType.normalDamage].Value;
            int damage = (int)(percentNormalDamage * characterStats.normalDamage.Value);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && 
                taticalMovement.currentIndex.IsOrtho(index)&& distance <=3)
            {
                if ((Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput) && taticalMovement.EnemyCheck(index) != null)
                {
                    InputHandler.instance.tacticsXInput = false;
                    target = taticalMovement.EnemyCheck(index);
                    taticalMovement.targetIndex = index;
                    characterStats.UseAP(skill.APcost);
                    targetPos = target.transform.position;
                    targetDirection = (target.transform.position - characterStats.transform.position).normalized;                    
                    animationHandler.PlayTargetAnimation("ShieldCharge");
                }
            }

            if (stateManager.characterAction== CharacterAction.ShieldCharge)
            {
                
                characterStats.transform.LookAt(target.transform);                
                Rigidbody characterRigidbody = characterStats.GetComponent<Rigidbody>();
                               
                if (stateManager.skillColliderTiggered)
                {
                    target.transform.LookAt(characterRigidbody.transform);
                    target.GetComponent<CharacterStats>().TakeDamage(damage);
                    target.GetComponent<TaticalMovement>().moveLocation = target.transform.position + 1.5f * targetDirection;
                    target.GetComponent<AnimationHandler>().PlayTargetAnimation("StumbleAndFall");
                    stateManager.skillColliderTiggered = false;                   
                }

                else
                {
                    characterRigidbody.velocity = 5f * targetDirection;
                }

                if ((targetPos - characterRigidbody.transform.position).magnitude < .2)
                {
                    characterRigidbody.velocity = Vector3.zero;
                    characterRigidbody.position = targetPos;
                    animationHandler.PlayTargetAnimation("CombatIdle");
                    taticalMovement.UpdateGridState();
                    taticalMovement.GetComponent<PlayerManager>().selectedSkill = null;

                    if (characterStats.currentAP != 0)
                        taticalMovement.SetCurrentNavDict();
                }
            }                            
        }

        public static void Activate(CharacterStats characterStats, AnimationHandler animationHandler,
            TaticalMovement taticalMovement, Skill skill, PlayerManager target, float delta)
        {
            IntVector2 index = target.taticalMovement.currentIndex;
            int distance = taticalMovement.GetRequiredMoves(index,taticalMovement.path);
            CharacterStateManager stateManager = characterStats.GetComponent<CharacterStateManager>();
            float percentNormalDamage = skill.combatStatScaleDict[CombatStatType.normalDamage].Value;
            int damage = (int)(percentNormalDamage * characterStats.normalDamage.Value);

            if (stateManager.characterAction != CharacterAction.ShieldCharge)
            {
                taticalMovement.targetIndex = index;
                characterStats.UseAP(skill.APcost);
                targetDirection = (target.transform.position - characterStats.transform.position).normalized;
                animationHandler.PlayTargetAnimation("ShieldCharge");
            }

            else
            {
                characterStats.transform.LookAt(target.transform);
                Vector3 targetPos = taticalMovement.mapAdapter.GetCellByIndex(taticalMovement.targetIndex).transform.position;
                Rigidbody characterRigidbody = characterStats.GetComponent<Rigidbody>();

                if (stateManager.skillColliderTiggered)
                {
                    target.transform.LookAt(characterRigidbody.transform);
                    target.GetComponent<CharacterStats>().TakeDamage(damage);
                    target.GetComponent<TaticalMovement>().moveLocation = targetPos + 1.5f * targetDirection;
                    target.GetComponent<AnimationHandler>().PlayTargetAnimation("StumbleAndFall");
                    stateManager.skillColliderTiggered = false;
                }

                else
                {
                    characterRigidbody.velocity = 5f * characterRigidbody.transform.forward;
                }

                if ((targetPos - characterRigidbody.transform.position).magnitude < .1)
                {
                    characterRigidbody.velocity = Vector3.zero;
                    characterRigidbody.position = targetPos;
                    animationHandler.PlayTargetAnimation("CombatIdle");
                    taticalMovement.UpdateGridState();
                }
            }
        }
    }
}

