using UnityEngine;

public class AttackTimerState : StateMachineBehaviour
{
    /*if the animation is more  than .25s of its time it'll
    put the isAttackMovPossible to false*/
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(AnimatorAshesh.isTargetLocked)) return;

        if (stateInfo.normalizedTime >= .25f)
            animator.SetBool(AnimatorAshesh.isAttackMovPossible, false);
    }

    /*as soon as the anmationExits the controlTime and 
    is attackMovPossible will switch valuel*/
    public override void OnStateExit(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorAshesh.controlTimerHandler, false);
        animator.SetBool(AnimatorAshesh.isAttackMovPossible, true);
    }

}
