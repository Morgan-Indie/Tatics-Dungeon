using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class ShieldChargeAnimation : StateMachineBehaviour
    {
        public ShieldCharge shieldCharge;
        public CharacterStateManager stateManager;
        public TaticalMovement taticalMovement;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            stateManager = animator.GetComponent<CharacterStateManager>();
            shieldCharge = animator.GetComponent<ShieldCharge>();
            taticalMovement = animator.GetComponent<TaticalMovement>();
            stateManager.characterAction = CharacterAction.ShieldCharge;
            stateManager.characterState = CharacterState.IsInteracting;
            shieldCharge.GetComponent<Collider>().isTrigger = true;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            shieldCharge.characterRigidBody.velocity = 5f * shieldCharge.targetDirection;

            if (taticalMovement.ReachedPosition(taticalMovement.transform.position, shieldCharge.targetPos)&& !shieldCharge.reachedTarget)
            {
                shieldCharge.EndCast();
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            stateManager.characterAction = CharacterAction.None;
            stateManager.characterState = CharacterState.Ready;
            shieldCharge.reachedTarget = false;
            shieldCharge.GetComponent<Collider>().isTrigger = false;
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }

}
