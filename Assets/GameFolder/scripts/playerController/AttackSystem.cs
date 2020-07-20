using UnityEngine;

namespace ControllerInputs
{
    public class AttackSystem : MonoBehaviour
    {
        private Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            wieldingWeapon();
            attacking();
        }

        private void setAttack(int attackType)
        {
            if (animator.GetBool(AnimatorAshesh.canAttack))
            {
                animator.SetInteger(AnimatorAshesh.attackType, attackType);
                animator.SetTrigger(AnimatorAshesh.attack);
                animator.SetBool(AnimatorAshesh.isAttacking, true);
            }
        }

        private void attacking()
        {
            if (Input.GetMouseButtonDown(0))
                setAttack(1);
            else if (Input.GetMouseButtonDown(1))
                setAttack(2);
        }

        private void wieldingWeapon()
        {
            if (Input.GetKeyDown(KeyCode.L))
                animator.SetBool(AnimatorAshesh.arming, !animator.GetBool(AnimatorAshesh.arming));
        }
    }
}