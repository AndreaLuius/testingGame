
using UnityEngine;

public class LooseAttackController : StateMachineBehaviour
{
    [SerializeField] float normalizedTime = 1f;
    private bool isInIt;

    public override void OnStateUpdate(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= normalizedTime)
        {
            if(axisControl("Vertical") || axisControl("Horizontal"))
            { 
                animator.SetBool(AnimatorAshesh.attMovAllower, true);
                isInIt = true;
            }
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorAshesh.attMovAllower, false);
        isInIt = false;
    }

    private bool axisControl(string axis)
    {
        return !isInIt && (Input.GetAxis(axis) >= .4
                 || Input.GetAxis(axis) <= -.4);
    }
}
