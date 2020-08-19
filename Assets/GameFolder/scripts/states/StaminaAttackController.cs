using UnityEngine;
using Properties;

public class StaminaAttackController : StateMachineBehaviour
{
    [SerializeField] Stamina stamina;

    void Start()
    {
    }

    private void OnStateEnter(
           Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
    }
}
