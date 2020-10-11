using System.Collections;
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
        public bool targetPathBlocked = false;
        IntVector2 preTargetCellIndex = new IntVector2(-1, -1);
        public List<GridCell> cells;

        public Rigidbody characterRigidBody;
        public CharacterStateManager stateManager;
        GridCell targetCell = null;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, CombatUtils _combatUtils, Skill _skill, SkillSlot _slot)
        {
            ShieldCharge shieldCharge = _characterStats.gameObject.AddComponent<ShieldCharge>();
            shieldCharge.characterStats = _characterStats;
            shieldCharge.animationHandler = _animationHandler;
            shieldCharge.taticalMovement = _taticalMovement;
            shieldCharge.characterRigidBody = _taticalMovement.GetComponent<Rigidbody>();
            shieldCharge.stateManager = _taticalMovement.GetComponent<CharacterStateManager>();
            shieldCharge.skill= _skill;
            shieldCharge.combatUtils = _combatUtils;
            shieldCharge.slot = _slot;
            return shieldCharge;
        }

        public void EndCast()
        {
            reachedTarget = true;
            //Excute(Time.deltaTime);
            characterRigidBody.velocity = Vector3.zero;
            characterRigidBody.position = targetPos;
            characterRigidBody.constraints = RigidbodyConstraints.FreezeAll;
            if (!targetPathBlocked)
                animationHandler.PlayTargetAnimation("CombatIdle");
            taticalMovement.SetCurrentNavDict();
            slot.DisableSkill();
            taticalMovement.GetComponent<PlayerManager>().selectedSkill = null;
            targetCell = null;
        }

        public override void Cast(float delta, IntVector2 _targetIndex)
        {
            targetIndex = _targetIndex;
            cells = PinnedShapes.GetPinnedCells(skill,taticalMovement.currentIndex , targetIndex);
            animationHandler.PlayTargetAnimation("ShieldCharge");
            characterRigidBody.constraints = RigidbodyConstraints.FreezeRotation| RigidbodyConstraints.FreezePositionY;
            characterStats.UseAP(skill.APcost);
            slot.skillCoolDownTurns += skill.coolDown;

            foreach (GridCell cell in cells)
            {
                if (cell.occupyingObject != null && cell.occupyingObject!=this.gameObject)
                {
                    target = cell.occupyingObject;
                    targetCell = cell;
                    break;
                }
            }

            if (targetCell!=null)
            {
                IntVector2 indexDirection = targetCell.index - taticalMovement.currentIndex;
                if (indexDirection.x == 0 && indexDirection.y > 0)
                {
                    GridCell nextCell = taticalMovement.mapAdapter.GetCellByIndex(targetCell.index + new IntVector2(0, 1));
                    if (nextCell.occupyingObject != null)
                    {
                        targetPathBlocked = true;
                        preTargetCellIndex = targetCell.index - new IntVector2(0, 1);
                    }
                }

                else if (indexDirection.x == 0 && indexDirection.y < 0)
                {
                    GridCell nextCell = taticalMovement.mapAdapter.GetCellByIndex(targetCell.index + new IntVector2(0, -1));
                    if (nextCell.occupyingObject != null)
                    {
                        targetPathBlocked = true;
                        preTargetCellIndex = targetCell.index - new IntVector2(0, -1);
                    }
                }

                else if (indexDirection.x > 0 && indexDirection.y == 0)
                {
                    GridCell nextCell = taticalMovement.mapAdapter.GetCellByIndex(targetCell.index + new IntVector2(1, 0));
                    if (nextCell.occupyingObject != null)
                    {
                        targetPathBlocked = true;
                        preTargetCellIndex = targetCell.index - new IntVector2(1, 0);
                    }
                }

                else if (indexDirection.x < 0 && indexDirection.y == 0)
                {
                    GridCell nextCell = taticalMovement.mapAdapter.GetCellByIndex(targetCell.index + new IntVector2(-1, 0));
                    if (nextCell.occupyingObject != null)
                    {
                        targetPathBlocked = true;
                        preTargetCellIndex = targetCell.index - new IntVector2(-1, 0);
                    }
                }
            }
            else
                targetCell = cells[cells.Count - 1];

            targetPos = targetCell.transform.position;

            targetDirection = (targetPos - taticalMovement.currentCell.transform.position);
            targetDirection.y = 0f;
            targetDirection.Normalize();
            characterStats.transform.LookAt(targetPos);
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            Debug.Log("Excute dispatch not implemented");
        }

        public override void Excute(float delta)
        {
            target.transform.LookAt(characterRigidBody.transform);
            int damage = (int)(characterStats.normalDamage.Value);
            target.GetComponent<CharacterStats>().TakeDamage(damage);
            if (!targetPathBlocked)
                target.GetComponent<TaticalMovement>().moveLocation = target.transform.position + 1.5f * targetDirection;
            else
            {
                if (!preTargetCellIndex.Equals(new IntVector2(-1, -1)))
                {
                    taticalMovement.moveLocation = taticalMovement.mapAdapter.GetPosByIndex(preTargetCellIndex);
                    target.GetComponent<TaticalMovement>().moveLocation = target.transform.position;
                    animationHandler.PlayTargetAnimation("StumbleAndFall");
                }
                else
                    Debug.Log("preTargetCellIndex not valid");
            }
            target.GetComponent<AnimationHandler>().PlayTargetAnimation("StumbleAndFall");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy" || other.tag == "Player")
                Excute(Time.deltaTime);
            if (targetPathBlocked)
                EndCast();
        }
    }
}

