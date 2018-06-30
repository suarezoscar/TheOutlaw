using UnityEngine;
using System.Collections;

public class ReloadBehaviour : StateMachineBehaviour {

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Character>().Reload = true;
        //animator.GetComponent<Character>().MeleeAttack();
        animator.SetFloat("speed", 0);

        if (animator.tag == "Player")
        {
            if (Player.Instance.OnGround && !Player.Instance.IsDead && !Player.Instance.Jump && !Player.Instance.Attack && !Player.Instance.TakingDamage)
            {
                Player.Instance.MyRigidbody.velocity = Vector2.zero;

            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("reload");
        animator.GetComponent<Character>().Reload = false;
        animator.GetComponent<Character>().BulletsNumber = animator.GetComponent<Player>().MaxBullets1;
        Player.Instance.NoAmmo = false;
        for (int i = 0; i < 6; i++)
        {
            animator.GetComponent<Character>().Bullets[i].SetActive(true);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
