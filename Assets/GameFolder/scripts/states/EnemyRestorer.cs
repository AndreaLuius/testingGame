using UnityEngine;

namespace states
{
    public class EnemyRestorer : StateMachineBehaviour
    {
        private void OnStateExit(
            Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            animator.SetInteger(AnimatorAshesh.attackType, 0);
            animator.SetBool(AnimatorAshesh.isInProcess, false);
        }
    }
}
