using UnityEngine;

public class EnemyAttackTimerState : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorAshesh.controlTimerHandler, false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime > .85)
            animator.SetBool(AnimatorAshesh.controlTimerHandler, true);   
    }
}
