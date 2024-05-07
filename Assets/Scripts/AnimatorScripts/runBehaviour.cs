using UnityEngine;

public class runBehaviour : StateMachineBehaviour
{
    private PlayerAttackController controller;
    private PlayerController controllerMovement;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.transform.parent.gameObject.
            transform.parent.gameObject.
            GetComponent<PlayerAttackController>();
        controllerMovement = animator.gameObject.transform.parent.gameObject.
            transform.parent.gameObject.
            GetComponent<PlayerController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controllerMovement.IsDefending == true)
        {
            animator.Play("Idle");
        }

        if (controller.IsAttacking)
        {
            controllerMovement.CanMove = false;
            controllerMovement.Rigidbody2D.velocity = Vector2.zero;
            controller.FirstAttackCollider.gameObject.SetActive(true);
            animator.Play("Attack1");
        }

        if (Mathf.Abs(controllerMovement.Direction) < 0.2)
        {
            animator.Play("Idle");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
