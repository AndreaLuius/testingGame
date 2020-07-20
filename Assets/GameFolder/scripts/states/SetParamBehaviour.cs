using UnityEngine;
using System;

public class SetParamBehaviour : StateMachineBehaviour
{
    [SerializeField] ParameterizedBehaviour[] parameterized;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (ParameterizedBehaviour par in parameterized)
            animator.SetBool(par.paramName, par.stateValue);
    }

    [Serializable]
    public struct ParameterizedBehaviour
    {
        public string paramName;
        public bool stateValue;
    }
}
