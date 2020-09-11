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
<<<<<<< Updated upstream
        public bool animationCompleted;

        public Rigidbody characterRigidBody;
        public CharacterStateManager stateManager;
        GridCell targetCell;
        bool isExcuting = false;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, Skill _skill)
        {
            ShieldCharge shieldCharge = _characterStats.gameObject.AddComponent<ShieldCharge>();
            shieldCharge.characterStats = _characterStats;
            shieldCharge.animationHandler = _animationHandler;
            shieldCharge.taticalMovement = _taticalMovement;
            shieldCharge.characterRigidBody = _taticalMovement.GetComponent<Rigidbody>();
            shieldCharge. stateManager = _animationHandler.stateManager;
            shieldCharge.skill= _skill;
            return shieldCharge;
        }

        public override void Activate(float delta)
        {
            if (!isExcuting)
            {
                IntVector2 index = taticalMovement.GetMouseIndex();
                int distance = index.GetDistance(taticalMovement.currentIndex);
=======
        public CharacterStateManager stateManager;
        public Rigidbody characterRigidbody;
        public CharacterStats characterStats;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;
        public bool excuting = false;
        GridCell targetCell;

        public override void AttachToCharacter(CharacterStats _characterStats,
            AnimationHandler _animationHandler, TaticalMovement _taticalMovement)
        {
            characterStats = _characterStats;
            taticalMovement = _taticalMovement;
            animationHandler = _animationHandler;

            characterRigidbody = taticalMovement.GetComponent<Rigidbody>();
            stateManager = characterStats.stateManager;
        }

        public override void Activate(float delta)
        {
            if (!excuting)
            {
                IntVector2 index = taticalMovement.GetMouseIndex();
                int distance = index.GetDistance(taticalMovement.currentIndex);

>>>>>>> Stashed changes
                if (index.x >= 0 && characterStats.currentAP >= skill.APcost &&
                    taticalMovement.currentIndex.IsOrtho(index) && distance <= 3)
                {
                    if ((Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput))
                    {
                        InputHandler.instance.tacticsXInput = false;
                        targetCell = taticalMovement.mapAdapter.GetCellByIndex(index);
<<<<<<< Updated upstream
=======
                        characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
>>>>>>> Stashed changes
                        Excute(delta, targetCell);
                    }
                }
            }
            else
                Excute(delta, targetCell);
<<<<<<< Updated upstream
        }

        public override void Excute(float delta, GridCell targetCell)
        {            
            if (stateManager.characterAction!=CharacterAction.ShieldCharge)
            {
                target = targetCell.GetOccupyingObject();
                characterRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
=======
       
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            if (stateManager.characterAction != CharacterAction.ShieldCharge)
            {
                target = targetCell.GetOccupyingObject();
                characterStats.UseAP(skill.APcost);

>>>>>>> Stashed changes
                if (target != null)
                    targetPos = target.transform.position;
                else
                    targetPos = targetCell.transform.position;

<<<<<<< Updated upstream
                characterStats.UseAP(skill.APcost);
                targetDirection = (targetPos - characterStats.transform.position).normalized;
                animationHandler.PlayTargetAnimation("ShieldCharge");
                isExcuting = true;
=======
                targetDirection = (targetPos - characterStats.transform.position).normalized;
                animationHandler.PlayTargetAnimation("ShieldCharge");
                excuting = true;
>>>>>>> Stashed changes
            }
            else
            {
                characterStats.transform.LookAt(target.transform);
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream

                else
                {
                    characterRigidBody.velocity = 5f * targetDirection;
=======
                else
                {
                    characterRigidbody.velocity = 5f * targetDirection;
>>>>>>> Stashed changes
                }
            }
        }
    }
}

