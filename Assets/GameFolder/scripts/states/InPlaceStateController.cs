using UnityEngine;

public class InPlaceStateController : StateMachineBehaviour
{

    private void OnStateExit(
          Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool(AnimatorAshesh.turnLeft, false);
        animator.SetBool(AnimatorAshesh.turnRight, false);
        animator.SetBool(AnimatorAshesh.bigTurnLeft, false);
        animator.SetBool(AnimatorAshesh.bigTurnRight, false);

       
    }

    /*only use if you wanna make it dynamic but not now! LOL*/

    //public Operational[] operational;

    //foreach (var i in operational)
    //animator.SetBool(i.animName, i.isTrue);

    //[Serializable]
    //public struct Operational
    //{
    //    public string animName;
    //    public bool isTrue;
    //}


}