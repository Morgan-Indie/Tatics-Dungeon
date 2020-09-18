﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class ShieldCharge : CastPhysical
    {
        public GameObject target=null;
        public Vector3 targetDirection;
        public Vector3 targetPos;
        public IntVector2 targetIndex;
        public bool reachedTarget=false;

        public Rigidbody characterRigidBody;
        public CharacterStateManager stateManager;
        GridCell targetCell = null;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill)
        {
            ShieldCharge shieldCharge = _characterStats.gameObject.AddComponent<ShieldCharge>();
            shieldCharge.characterStats = _characterStats;
            shieldCharge.animationHandler = _animationHandler;
            shieldCharge.taticalMovement = _taticalMovement;
            shieldCharge.characterRigidBody = _taticalMovement.GetComponent<Rigidbody>();
            shieldCharge.stateManager = _taticalMovement.GetComponent<CharacterStateManager>();
            shieldCharge.skill= _skill;
            shieldCharge.combatUtils = _combatUtils;
            return shieldCharge;
        }

        public void EndCast()
        {
            reachedTarget = true;
            //Excute(Time.deltaTime);
            characterRigidBody.velocity = Vector3.zero;
            characterRigidBody.position = targetPos;
            characterRigidBody.constraints = RigidbodyConstraints.FreezeAll;
            animationHandler.animator.SetBool("TransitionToCombatIdle", true);
            taticalMovement.UpdateGridState();
            taticalMovement.GetComponent<PlayerManager>().selectedSkill = null;
            taticalMovement.SetCurrentNavDict();
            targetCell = null;
        }

        public override void Cast(float delta, IntVector2 _targetIndex)
        {
            targetIndex = _targetIndex;
            List<GridCell> cells = PinnedShapes.GetPinnedCells(skill,taticalMovement.currentIndex , targetIndex);
            animationHandler.PlayTargetAnimation("ShieldCharge");
            characterRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            characterStats.UseAP(skill.APcost);

            foreach (GridCell cell in cells)
            {
                if (cell.occupyingObject != null && cell.occupyingObject!=this.gameObject)
                {
                    target = cell.occupyingObject;
                    targetCell = cell;
                    break;
                }
            }

            if (targetCell == null)
                targetCell = cells[cells.Count-1];

            targetPos = targetCell.transform.position;
            targetCell.index.Print();
            targetDirection = (targetPos - taticalMovement.currentCell.transform.position);
            targetDirection.y = 0f;
            targetDirection.Normalize();
            characterStats.transform.LookAt(targetPos);
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            target.transform.LookAt(characterRigidBody.transform);
            int damage = (int)(characterStats.normalDamage.Value);
            target.GetComponent<CharacterStats>().TakeDamage(damage);
            target.GetComponent<TaticalMovement>().moveLocation = target.transform.position + 1.5f * targetDirection;
            target.GetComponent<AnimationHandler>().PlayTargetAnimation("StumbleAndFall");
        }

        public override void Excute(float delta)
        {
            target.transform.LookAt(characterRigidBody.transform);
            int damage = (int)(characterStats.normalDamage.Value);
            target.GetComponent<CharacterStats>().TakeDamage(damage);
            target.GetComponent<TaticalMovement>().moveLocation = target.transform.position + 1.5f * targetDirection;
            target.GetComponent<AnimationHandler>().PlayTargetAnimation("StumbleAndFall");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy" || other.tag == "Player")
                Excute(Time.deltaTime);
        }
    }
}
