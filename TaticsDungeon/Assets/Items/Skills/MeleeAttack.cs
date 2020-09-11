using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class MeleeAttack : SkillAbstract
    {
        public bool animationCompleted;
<<<<<<< Updated upstream
        public GameObject target;

        public override SkillAbstract AttachSkill(CharacterStats _characterStats, AnimationHandler _animationHandler,
            TaticalMovement _taticalMovement, Skill _skill)
        {
            MeleeAttack meleeAttack = _characterStats.gameObject.AddComponent<MeleeAttack>();
            meleeAttack.characterStats = _characterStats;
            meleeAttack.animationHandler = _animationHandler;
            meleeAttack.taticalMovement = _taticalMovement;
            meleeAttack.skill = _skill;
            return meleeAttack;
=======
        public CharacterStats characterStats;
        public AnimationHandler animationHandler;
        public TaticalMovement taticalMovement;

        public override void AttachToCharacter(CharacterStats _characterStats,
            AnimationHandler _animationHandler, TaticalMovement _taticalMovement)
        {
            characterStats = _characterStats;
            taticalMovement = _taticalMovement;
            animationHandler = _animationHandler;
>>>>>>> Stashed changes
        }

        public override void Activate(float delta)
        {
            IntVector2 index = taticalMovement.GetMouseIndex();
            int distance = taticalMovement.currentIndex.GetDistance(index);

            if (index.x >= 0 && characterStats.currentAP >= skill.APcost && distance == 1)
            {
                if (Input.GetMouseButtonDown(0) || InputHandler.instance.tacticsXInput && 
                    characterStats.stateManager.characterState!= CharacterState.IsInteracting)
                {
                    InputHandler.instance.tacticsXInput = false;
                    GridCell targetCell = taticalMovement.mapAdapter.GetCellByIndex(index);

                    Excute(delta, targetCell);
                }
            }
        }

        public override void Excute(float delta, GridCell targetCell)
        {
            target = targetCell.GetOccupyingObject();
            if (target != null)
                characterStats.transform.LookAt(target.transform);
                animationHandler.PlayTargetAnimation("Attack");
                characterStats.GetComponent<CombatUtils>().PhyiscalAttack(target);
                characterStats.UseAP(skill.APcost);
        }
    }       
}

