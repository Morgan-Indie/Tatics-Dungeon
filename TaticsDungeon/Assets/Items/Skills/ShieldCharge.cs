using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class ShieldCharge : SkillAbstract
    {
        public GameObject target=null;
        public Vector3 targetDirection;
        public Vector3 targetPos;

        public override void Activate(CharacterStats characterStats,AnimationHandler animationHandler, 
            TaticalMovement taticalMovement, float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            int distance = index.GetDistance(taticalMovement.currentIndex);
            CharacterStateManager stateManager = characterStats.GetComponent<CharacterStateManager>();

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && 
                taticalMovement.currentIndex.IsOrtho(index)&& distance <=3)
            {
                if ((Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput))
                {
                    InputHandler.instance.tacticsXInput = false;
                    GridCell targetCell = taticalMovement.mapAdapter.GetCellByIndex(index);
                    Excute(characterStats, animationHandler, taticalMovement, delta, targetCell);
                }
            }            
        }

        public override void Excute(CharacterStats characterStats,
            AnimationHandler animationHandler, TaticalMovement taticalMovement,
            float delta, GridCell targetCell)
        {
            CharacterStateManager stateManager = characterStats.GetComponent<CharacterStateManager>();

            if (stateManager.characterAction != CharacterAction.ShieldCharge)
            {

                GameObject target = targetCell.GetOccupyingObject();
                characterStats.UseAP(skill.APcost);

                if (target != null)
                    targetPos = target.transform.position;
                else
                    targetPos = targetCell.transform.position;

                targetDirection = (target.transform.position - characterStats.transform.position).normalized;
                animationHandler.PlayTargetAnimation("ShieldCharge");
            }

            else
            {
                characterStats.transform.LookAt(target.transform);
                Rigidbody characterRigidbody = characterStats.GetComponent<Rigidbody>();

                if (stateManager.skillColliderTiggered)
                {
                    if (target.tag == "Enemy" || target.tag == "Player")
                    {
                        target.transform.LookAt(characterRigidbody.transform);
                        float percentNormalDamage = skill.combatStatScaleDict[CombatStatType.normalDamage].Value;
                        int damage = (int)(percentNormalDamage * characterStats.normalDamage.Value);
                        target.GetComponent<CharacterStats>().TakeDamage(damage);
                        target.GetComponent<TaticalMovement>().moveLocation = target.transform.position + 1.5f * targetDirection;
                        target.GetComponent<AnimationHandler>().PlayTargetAnimation("StumbleAndFall");
                        stateManager.skillColliderTiggered = false;
                    }
                }

                else
                {
                    characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    characterRigidbody.velocity = 5f * targetDirection;
                }

                if (taticalMovement.ReachedPosition(taticalMovement.transform.position, targetPos))
                {
                    characterRigidbody.velocity = Vector3.zero;
                    characterRigidbody.position = targetPos;
                    characterRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    animationHandler.PlayTargetAnimation("CombatIdle");
                    taticalMovement.UpdateGridState();
                    taticalMovement.GetComponent<PlayerManager>().selectedSkill = null;

                    taticalMovement.SetCurrentNavDict();
                }
            }
        }
    }
}

