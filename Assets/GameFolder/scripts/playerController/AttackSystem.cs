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

        /**
        Checks if the player is allowed to attack
        and if he is, starts the attack process
        */
        private void setAttack(int attackType)
        {
            if (animator.GetBool(AnimatorAshesh.canAttack))
            {
                animator.SetInteger(AnimatorAshesh.attackType, attackType);
                animator.SetTrigger(AnimatorAshesh.attack);
                animator.SetBool(AnimatorAshesh.isAttacking, true);
            }
        }

        /**
        Filters the attack based on Light Attack(1)
        and heavy Attack(2)
        */
        private void attacking()
        {
            if (Input.GetMouseButtonDown(0))
                setAttack(1);
            else if (Input.GetMouseButtonDown(1))
                setAttack(2);
        }

        /**
        Allow the player to Wield a weapon
        and trigger its animation*/
        private void wieldingWeapon()
        {
            if (Input.GetKeyDown(KeyCode.L))
                animator.SetBool(AnimatorAshesh.arming, !animator.GetBool(AnimatorAshesh.arming));
        }
    }
}