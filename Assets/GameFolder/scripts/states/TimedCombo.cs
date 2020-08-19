using UnityEngine;

public class TimedCombo : StateMachineBehaviour
{
    [SerializeField] string paramName;
    [SerializeField] bool currentValue;
    [SerializeField] float startTime, endTime;
    private bool isWaitingForExit;
    private bool isOnTrasitionExit;

    private void OnStateEnter(
        Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        isWaitingForExit = false;
        isOnTrasitionExit = false;
        animator.SetBool(AnimatorAshesh.isAttacking, true);

        changeAnimatorState(animator);
        //
        animator.SetBool(AnimatorAshesh.staminaSucker, true);
    }

    private void OnStateUpdate(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (checkTrasitionExit(animator, layerIndex))
        {
            changeAnimatorState(animator);
            animator.SetBool(AnimatorAshesh.isAttacking, false);
        }

        if (!isOnTrasitionExit &&
                stateInfo.normalizedTime >= startTime && stateInfo.normalizedTime <= endTime)
            animator.SetBool(paramName, currentValue);
    }

    private void OnStateExit(
        Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!isOnTrasitionExit) changeAnimatorState(animator);
    }

    #region SelfFunction
    private void changeAnimatorState(Animator animator)
    {
        animator.SetBool(paramName, !currentValue);
    }

    private bool checkTrasitionExit(Animator animator, int layerIndex)
    {
        if (!isWaitingForExit &&
                 animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash == 0)
            isWaitingForExit = true;

        if (!isOnTrasitionExit &&
            isWaitingForExit && animator.IsInTransition(layerIndex))
        {
            isOnTrasitionExit = true;
            return true;
        }

        return false;
    }
    #endregion


}