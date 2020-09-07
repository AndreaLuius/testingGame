using UnityEngine;

public class RollController : StateMachineBehaviour
{
    private void OnStateEnter(
       Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorAshesh.staminaSucker, true);
    }
}