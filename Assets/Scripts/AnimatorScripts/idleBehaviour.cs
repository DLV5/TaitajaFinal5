using UnityEngine;
using UnityEngine.InputSystem.XR;

public class idleBehaviour : StateMachineBehaviour
{
    private PlayerAttackController controller;
    private PlayerController controllerMovement;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<PlayerAttackController>();
        controllerMovement = animator.gameObject.GetComponent<PlayerController>();
        controllerMovement.CanMove = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.IsAttacking)
        {
            controllerMovement.CanMove = false;
            controller.FirstAttackCollider.gameObject.SetActive(true);
            animator.Play("Attack1");
        }

        if (Mathf.Abs(controllerMovement.Direction) > 0.2)
        {
            animator.Play("Run");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.IsAttacking = false;
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
