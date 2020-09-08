using System;
using UnityEngine;

namespace AssemblyCSharp.Assets.GameFolder.scripts.states
{
    public class EnemyRestorer : StateMachineBehaviour
    {
        private void OnStateExit(
            Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            animator.SetInteger(AnimatorAshesh.attackType, 0);
        }
    }
}
