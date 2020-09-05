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
        public bool animationCompleted;
        public CharacterStats characterStats;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;
        public Rigidbody characterRigidBody;
        public CharacterStateManager stateManager;
        GridCell targetCell;
        bool isExcuting = false;

        public override void AttachToCharacter(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement)
        {
            characterStats = _characterStats;
            animationHandler = _animationHandler;
            taticalMovement = _taticalMovement;

            characterRigidBody = taticalMovement.GetComponent<Rigidbody>();
            stateManager = animationHandler.stateManager;
        }

        public override void Activate(float delta)
        {
            if (!isExcuting)
            {
                IntVector2 index = taticalMovement.GetMouseIndex();
                int distance = index.GetDistance(taticalMovement.currentIndex);

                if (index.x >= 0 && characterStats.currentAP >= skill.APcost &&
                    taticalMovement.currentIndex.IsOrtho(index) && distance <= 3)
                {
                    if ((Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput))
                    {
                        InputHandler.instance.tacticsXInput = false;
                        targetCell = taticalMovement.mapAdapter.GetCellByIndex(index);
                        Excute(delta, targetCell);
                    }
                }
            }
            else
                Excute(delta, targetCell);
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            if (stateManager.characterAction != CharacterAction.ShieldCharge)
            {

                GameObject target = targetCell.GetOccupyingObject();
                characterStats.UseAP(skill.APcost);

                if (target != null)
                    targetPos = target.transform.position;
                else
                    targetPos = targetCell.transform.position;

                targetDirection = (targetPos - characterStats.transform.position).normalized;
                animationHandler.PlayTargetAnimation("ShieldCharge");
                characterRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
                isExcuting = true;
            }

            else
            {
                characterStats.transform.LookAt(target.transform);
                if (stateManager.skillColliderTiggered)
                {
                    if (target.tag == "Enemy" || target.tag == "Player")
                    {
                        target.transform.LookAt(characterRigidBody.transform);
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
                    characterRigidBody.velocity = 5f * targetDirection;
                }

                if (taticalMovement.ReachedPosition(taticalMovement.transform.position, targetPos))
                {
                    characterRigidBody.velocity = Vector3.zero;
                    characterRigidBody.position = targetPos;
                    characterRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                    animationHandler.PlayTargetAnimation("CombatIdle");
                    taticalMovement.UpdateGridState();
                    taticalMovement.GetComponent<PlayerManager>().selectedSkill = null;
                    isExcuting = false;

                    taticalMovement.SetCurrentNavDict();
                }
            }
        }
    }
}

