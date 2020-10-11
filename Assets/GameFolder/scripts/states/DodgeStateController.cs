using UnityEngine;

public class DodgeStateController : StateMachineBehaviour
{
    private void OnStateExit(
           Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorAshesh.dodgeBack, false);
        animator.SetBool(AnimatorAshesh.jumpTowards, false);
    }
}
