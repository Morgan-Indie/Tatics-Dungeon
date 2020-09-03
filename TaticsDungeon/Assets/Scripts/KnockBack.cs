using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class KnockBack : StateMachineBehaviour
    {
        Vector3 targetPosition;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            TaticalMovement taticalMovement = animator.GetComponent<TaticalMovement>();
            targetPosition = taticalMovement.moveLocation;
            animator.GetComponent<CharacterStateManager>().characterAction = CharacterAction.LyingDown;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Vector3 currentVelocity = Vector3.zero;
            animator.transform.position = Vector3.SmoothDamp(animator.transform.position,
                targetPosition, ref currentVelocity, Time.deltaTime*4f);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            TaticalMovement taticalMovement = animator.GetComponent<TaticalMovement>();
            animator.transform.position = targetPosition;
            taticalMovement.moveLocation = Vector3.up;
            targetPosition = Vector3.up;
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

