using UnityEngine;

public class InPlaceStateController : StateMachineBehaviour
{
    private void OnStateExit(
          Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorAshesh.turnLeft, false);
        animator.SetBool(AnimatorAshesh.turnRight, false);
    }
}