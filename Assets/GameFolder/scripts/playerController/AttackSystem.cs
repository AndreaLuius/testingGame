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
            if (animator.GetBool("canAttack"))
            {
                animator.SetInteger("attackType", attackType);
                animator.SetTrigger("attack");
                animator.SetBool("isAttacking", true);
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
                animator.SetBool("arming", !animator.GetBool("arming"));
        }
    }
}