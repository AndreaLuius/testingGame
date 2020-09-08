using UnityEngine;

public class RollController : StateMachineBehaviour
{
    private void OnStateEnter(
       Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorAshesh.staminaSucker, true);
        animator.SetBool(AnimatorAshesh.isCurrentlyRolling, true);
    }

    private void OnStateExit(
       Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorAshesh.isCurrentlyRolling, false);
    }
}